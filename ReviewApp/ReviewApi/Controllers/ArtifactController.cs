using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReviewApi.Models.Database;
using ReviewApi.Models.Artifact;
using System.Net;
using ReviewApi.BusinessLogic;

namespace ReviewApi.Controllers
{
    //[Produces("application/json")]
    [Route("api/Artifact")]
    public class ArtifactController : Controller
    {
        ReviewsDatabaseContext context;
        public ArtifactController(ReviewsDatabaseContext context)
        {
            this.context = context;
        }
        [Route("GetArtifactsFormUrl")]
        [HttpPost]
        public IActionResult GetArtifactsFormUrl([FromBody]IbmUrlModel model)
        {
            string xml;
            List<JazzArtifact> nodesInXml = null;
            using (WebClient client = new WebClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                String username = model.Name;
                String password = model.Password;
                String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
                client.Headers.Add("Authorization", "Basic " + encoded);
                xml = client.DownloadString(model.Url);
                
                nodesInXml = XmlParser.CreateJazzObjects(xml);
                SaveArtifactToDatabase(nodesInXml, model);
            }
            return Ok();
        }
        private void SaveArtifactToDatabase(List<JazzArtifact> artifacts, IbmUrlModel model)
        {
            ;
            foreach(var a in artifacts)
            {
                IbmArtifact artifact = new IbmArtifact() { IbmId = a.IbmId, Name = a.Name, Url = a.Url, WorkproductId = model.WorkProductId };
                context.Workproduct.Where(w => w.Id == model.WorkProductId).FirstOrDefault().IbmArtifact.Add(artifact);
                
            }
            context.SaveChanges();
        }
        [HttpGet]
        [Route("GetAllArtifactForWorkProduct")]
        public IEnumerable<JazzArtifact> GetAllArtifactForWorkProduct(int id)
        {
            List<JazzArtifact> artifacts = new List<JazzArtifact>();
            foreach (var a in context.IbmArtifact.Where(ar => ar.WorkproductId == id).ToList())
            {
                JazzArtifact artifact = new JazzArtifact()
                {
                    IbmId = a.IbmId,
                    Id = a.Id,
                    Name = a.Name,
                    Url = a.Url
                };
                artifacts.Add(artifact);
            }
            return artifacts;
        }
        [HttpGet]
        [Route("GetArtifactsPerPage")]
        public IEnumerable<JazzArtifact> GetArtifactsForPage(int workProductId, int page)
        {
            int artifactPerPage = 15;
            int totalArtifacts = context.IbmArtifact.Count(a => a.WorkproductId == workProductId);
            
            int amoutOfPages = totalArtifacts / artifactPerPage;
            if (totalArtifacts % artifactPerPage > 0)
                amoutOfPages++;
            int skip = page * artifactPerPage;
            List<JazzArtifact> artifacts = new List<JazzArtifact>();
            var artifactsPerPage = context.IbmArtifact.Where(a => a.WorkproductId == workProductId).Skip(skip).Take(artifactPerPage).ToList();
            foreach (var a in artifactsPerPage)
            {
                JazzArtifact artifact = new JazzArtifact()
                {
                    IbmId = a.IbmId,
                    Id = a.Id,
                    Name = a.Name,
                    Url = a.Url
                };                 
                artifacts.Add(artifact);
            }
            return artifacts;
        }
        [HttpGet]
        [Route("NumberOfArtifactsInWorkProduct")]
        public int CountOfAllArtifactsInWorkProduct(int workProductId)
        {
            
            int artifactCount = context.IbmArtifact.Count(a => a.WorkproductId == workProductId);
            return artifactCount;
        }
        /*
        [HttpGet]
        [Route("GetArtifactDetailsHeadersForProject")]
        public List<string> GetArtifactDetailsHeadersForProject(int projectId)
        {
            Artifact artifact = context.Artifact.Where(a => a.ProjectId == projectId).FirstOrDefault();
            List<string> artifactDetails = new List<string>();
            var detailsNames = context.ArtifactDetail.Where(d => d.ArtifactId == artifact.Id).ToList();
            foreach (var d in detailsNames)
            {
                artifactDetails.Add(d.Name);
            }
            return artifactDetails;
        }*/
    }
}
//}