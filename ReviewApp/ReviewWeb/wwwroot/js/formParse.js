var Formdata;
function getReviewTameplate() {
    var tmp = "{\"model\":[{\"headr\":\"Column1\",\"type\":\"textarea\"},{\"headr\":\"Column 2\",\"type\":\"select\",\"option\":[\"Ano\",\"Ne\",\"Mozna\"]},{\"headr\":\"Column 3\",\"type\":\"textarea\"}],\"Name\":\"Tameplate1\",\"Descritpion\":\"\",\"header\":[{\"name\":\"Count all Ne\",\"fcn\":\"sum\",\"parameter\":\"Ne\"}],\"role\":[\"Reviewer\",\"Scribe\",\"Leader\"]}";
    var tameplate = JSON.parse(tmp);
    console.log(tameplate);
    createReviewForm(tameplate);
    return tameplate;
}

function createSelectForColumnData(id) {
    var table = document.getElementById('dataColumns');
    deleteRowsFromTable(table);
    if (typeof Formdata !== 'undefined') {
        var headers = getArtifactHeaders(id);

        /*if (typeof headers !== 'undefined') {
            console.log('headers pass');*/

            for (var i = 0; i < Formdata.columns.length; i++) {
                if (Formdata.columns[i].type === 'textarea') {
                    var row = document.createElement('tr');
                    var td1 = document.createElement('td');
                    var name = document.createTextNode(Formdata.columns[i].columnName);
                    td1.appendChild(name);
                    var select = slct();//createSelectForArtifactDetail(headers);
                    var td2 = document.createElement('td');
                    td2.appendChild(select);
                    row.appendChild(td1);
                    row.appendChild(td2);
                    table.appendChild(row);
                }
            }
        //}
    }
}
function deleteRowsFromTable(table) {
    while (table.hasChildNodes()) {
        table.removeChild(table.firstChild);
    }
}
function slct() {
    var select = document.createElement('select');
    var optionNames = ['','name', 'ibmId'];
    for (var i = 0; i < optionNames.length; i++) {
        var o = document.createElement('option');
        o.text = optionNames[i];
        o.value = optionNames[i];
        select.appendChild(o);
    }
    return select;
}
function createSelectForArtifactDetail(headers) {

    var select = document.createElement('select');
    var o = document.createElement('option');
    select.appendChild(o);
    for (var i = 0; i < headers.length; i++) {
        var option = document.createElement('option');
        option.value = headers[i];
        option.text = headers[i];
        select.appendChild(option);
    }
    return select;
}

function getArtifactHeaders(id) {
    var headers;
    $.ajax({
        type: 'GET',
        async: false,
        url: "/Artifact/GetArtifctsHeadersForProject?projectId=" + id,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result !== null) {
                //console.log(result);
                //nodes = JSON.parse(result);
                console.log(result);
                headers = result;

            } else {
                //document.getElementById("result").innerHTML = "NULL";
                alert("KO");
            }
        }
    });
    return headers;
}

function createForm() {
    var workProduct = document.getElementById('workProduct');
    var id = workProduct.options[workProduct.selectedIndex].value;
    var table = document.getElementById('Review');
    var length = table.rows.length;
    for (var i = 0; i < length; i++) {
        table.deleteRow(0);
    }
    var row = document.createElement('tr');
    row.id = "ReviewHeaderRow";
    table.appendChild(row);
    var select = document.getElementById('tameplateSelect');
    var selectValue = select.options[select.selectedIndex].value;
    Formdata = getTameplate(selectValue);
    if (typeof Formdata !== 'undefined') {
        createReviewForm(Formdata);
        var selects = document.getElementById('participants').getElementsByTagName('select');
        var s = createSelectForRoles(Formdata.roles);
        for (var i = 0; i < selects.length; i++) {
            var s = createSelectForRoles(Formdata.roles);

            var parrent = selects[i].parentNode;

            parrent.removeChild(selects[i]);

            parrent.appendChild(s);
        }
    }
    var empty = document.getElementById('reviewData');
    var tb = document.getElementById('ArtifactTable').getElementsByTagName('tbody');
    allArtifacts.destroy();
    deleteRowsFromTable(tb[0]);
    allArtifacts = $('#ArtifactTable').DataTable({ paging: true, "scrollX": true, destroy: true });
    var tble = document.getElementById('dataColumns');
    deleteRowsFromTable(tble);
    if (empty.checked !== true) {
        createSelectForColumnData(id);
        createArtifactTable(id);
    }
    
    //getArtifacts(id);
}
function createReviewForm(data) {
    if (typeof data !== 'undefined') {
        for (var i = 0; i < data.columns.length; i++) {
            createTableHeaderForColumn(data.columns[i].columnName);//okeej

        }
        createColumnsElements(data);
    }
}
function fillTextAreaInReview() {
    var table = document.getElementById('Review');
    var textarea = table.rows[1].cells[0].getElementsByTagName('textarea');
    var text = document.createTextNode('Artefact 1');
    textarea[0].appendChild(text);
    //console.log(textarea);
}
function createRowsInReviewForm() {
    var row = createColumnsElements(data);
    var table = document.getElementById("Review");
    var clone = row.cloneNode(true);
    table.appendChild(clone);
}

function createTableHeaderForColumn(name) {
    var tableRow = document.getElementById('ReviewHeaderRow');
    var th = document.createElement('th');
    var text = document.createTextNode(name);
    th.appendChild(text);
    tableRow.appendChild(th);
}

function createColumnsElements(data) {
    var table = document.getElementById("Review");
    var tr = document.createElement('tr');
    for (var i = 0; i < data.columns.length; i++) {
        if (data.columns[i].type === 'textarea') {
            var td = document.createElement('td');
            var textarea = document.createElement('textarea');
            td.appendChild(textarea);
            tr.appendChild(td);
        }
        else {
            var select = document.createElement('select');
            var td = document.createElement('td');
            createOptions(select, data.columns[i].option);
            td.appendChild(select);
            tr.appendChild(td);
        }
    }
    table.appendChild(tr);
    return tr;
}

function createOptions(select, data) {
    for (var i = 0; i < data.length; i++) {
        var option = document.createElement('option');
        option.value = data[i];
        var text = document.createTextNode(data[i]);
        option.appendChild(text);
        select.appendChild(option);
    }
}

function getTameplate(id) {
    console.log("get template fnc");
    var tameplate;
    $.ajax({
        type: 'GET',
        async: false,
        url: "/Template/GetTemplate?id=" + id, // http://localhost:60000/api/upload/ -- na tuto URL se budou posilat diagramy (XML)
        dataType: 'json',
        contentType: "application/json; charset=utf-8",

        success: function (result) {
            if (result !== null) {
                //console.log(result);
                //nodes = JSON.parse(result);
                console.log(result);
                tameplate = result;

            } else {
                //document.getElementById("result").innerHTML = "NULL";
                alert("KO");
            }
        }
    });
    return tameplate;
}

function getArtifacts(id) {
    var artifacts;
    $.ajax({
        type: 'GET',
        async: false,
        url: "/Artifact/GetAllArtifactForProject?projectId=" + id,
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            if (result !== null) {
                console.log(result);
                artifacts = result;

            } else {

                alert("KO");
            }
        }
    });
    return artifacts;

}
var art;
function createArtifactTable(id) {
    var artifacts = getArtifacts(id);
    console.log(artifacts);
    art = artifacts;
    var table = document.getElementById('ArtifactTable').getElementsByTagName('tbody');
    //var dltTbl = document.getElementById('ArtifactTable');
    //if (dltTbl.rows.length > 1)
    //artifactsForReview.clear();
    allArtifacts.destroy();
    deleteRowsFromTable(table[0]);
    for (var i = 0; i < artifacts.length; i++) {
        var row = createRowForArtifact(artifacts[i]);
        //allArtifacts.row.add(row);
        table[0].appendChild(row);
    }
    allArtifacts = $('#ArtifactTable').DataTable({ paging: true, "scrollX": true, destroy: true });
    //allArtifacts.draw();
}
function createRowForArtifact(data) {
    var row = document.createElement('tr');
    var td = document.createElement('td');
    var id = document.createTextNode(data.id);
    td.appendChild(id);
    var td2 = document.createElement('td');
    var a = document.createElement('a');
    a.href = data.url;
    a.target = "blank";
    var url = document.createTextNode(data.name);
    a.appendChild(url);
    td2.appendChild(a);
    var td3 = document.createElement('td');
    var btn = document.createElement('button');
    var btnText = document.createTextNode('Add');
    var span = document.createElement('i');

    span.setAttribute('class', "fas fa-plus-circle");
    btn.appendChild(span);

    btn.appendChild(btnText);
    btn.setAttribute('onclick', 'addArtifactToReview(this);');
    btn.setAttribute('class', 'btn btn-primary');
    td3.appendChild(btn);

    row.appendChild(td);
    row.appendChild(td2);
    row.appendChild(td3);
    return row;
}
/*
function addArtifactToReview(id) {
    var row = id.parentNode.parentNode;
    var table = document.getElementById('artifactsForReview').getElementsByTagName('tbody');
    row.deleteCell(2);
    var cell = row.insertCell(2);
    var btn = document.createElement('button');
    var btnText = document.createTextNode('Remove');
    btn.appendChild(btnText);
    btn.setAttribute('onclick', 'removeArtifactFromReview(this);');
    cell.appendChild(btn);
    table[0].appendChild(row);
}
function removeArtifactFromReview(id) {
    var row = id.parentNode.parentNode;
    var table = document.getElementById('ArtifactTable').getElementsByTagName('tbody');
    row.deleteCell(2);
    var cell = row.insertCell(2);
    var btn = document.createElement('button');
    var btnText = document.createTextNode('Add');
    btn.appendChild(btnText);
    btn.setAttribute('onclick', 'addArtifactToReview(this);');
    cell.appendChild(btn);
    table[0].appendChild(row);
}*/
function addArtifactToReview(id) {
    var index = id.parentNode.parentNode.rowIndex;
    console.log(index);
    var row = allArtifacts.row(index - 1).data();
    var btn = document.createElement('button');
    var btnText = document.createTextNode('Remove');
    var span = document.createElement('i');
    
    span.setAttribute('class', "far fa-times-circle");
    btn.appendChild(span);
    btn.appendChild(btnText);
    btn.setAttribute('onclick', 'removeArtifactFromReview(this);');
    btn.setAttribute('class', 'btn btn-danger');
    var div = document.createElement('div');
    div.appendChild(btn);
    row[2] = div.innerHTML;
    allArtifacts.row(index -1).remove().draw();
    artifactsForReview.row.add(row).draw();
}
function removeArtifactFromReview(id) {
    console.log(id);
    var index = id.parentNode.parentNode.rowIndex;
    console.log(index);
    var row = artifactsForReview.row(index - 1).data();
    console.log(row);
    var btn = document.createElement('button');
    var btnText = document.createTextNode('Add');
    var span = document.createElement('i');
    
    span.setAttribute('class', "fas fa-plus-circle");
    btn.appendChild(span);
    btn.appendChild(btnText);
    btn.setAttribute('onclick', 'addArtifactToReview(this);');
    btn.setAttribute('class', 'btn btn-primary');
    var div = document.createElement('div');
    div.appendChild(btn);
    row[2] = div.innerHTML;
    artifactsForReview.row(index - 1).remove().draw();
    allArtifacts.row.add(row).draw();
}

