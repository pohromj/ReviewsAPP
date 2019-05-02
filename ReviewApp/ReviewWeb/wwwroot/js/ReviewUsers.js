
var addedUsers = [];
var removedUsers = [];
function AppendToParticipantsTable(index) {
    console.log("AppendToParticipantsTable");
    var table = document.getElementById("participants").getElementsByTagName('tbody')[0];
    var row = index.parentNode.parentNode;
    var i = index.parentNode.parentNode.rowIndex;
    var indx = index.parentNode.cellIndex - 1;
    var tbl = document.getElementById("allUsers");
    var login = tbl.rows[i].cells[indx].innerText;
    row.deleteCell(3);
    //var cell = row.insertCell(3);
    //
        var selCel = row.insertCell(3);
        //console.log(Formdata.roles);
        if (typeof Formdata !== 'undefined') {
            var s = createSelectForRoles(Formdata.roles);
            selCel.appendChild(s);
        }
        else {
            var s = createSelectForRoles([]);
            selCel.appendChild(s);
        }
        var cell = row.insertCell(4);
        var btn = createButton('AppendToAllUsers(this)', 'Remove');
        btn.setAttribute('class', 'btn btn-primary btn-sm');
        cell.appendChild(btn);
        table.appendChild(row);


    console.log(btn);
    
    console.log("cell: " + indx);
    console.log("row: " + i);

    deleteFromUserArray(removedUsers, login)
    addedUsers.push(login);

    console.log("removed :" + removedUsers);
    console.log("added :" + addedUsers);
}
function AppendToAllUsers(index) {
    console.log("AppendToAllUsers");
    var table = document.getElementById("allUsers").getElementsByTagName('tbody')[0];
    var row = index.parentNode.parentNode;
    var i = index.parentNode.parentNode.rowIndex;
    console.log(i);
    var indx = index.parentNode.cellIndex - 1;
    console.log(indx);
    var tbl = document.getElementById("participants");
    var login = tbl.rows[i].cells[indx].innerText;
    console.log(login);
    row.deleteCell(4);
    row.deleteCell(3);
    var cell = row.insertCell(3);
    var btn = createButton('AppendToParticipantsTable(this)', 'Add');
    btn.setAttribute('class', 'btn btn-primary btn-sm');
    cell.appendChild(btn);
    table.appendChild(row);
    // var indx = index.parentNode.cellIndex;// - 1;
    console.log("cell: " + indx);
    console.log("row: " + i);
    deleteFromUserArray(addedUsers, login);
    removedUsers.push(login);
    console.log("removed :" + removedUsers);
    console.log("added :" + addedUsers);
    //console.log(data);
}
function createSelectForRoles(roles) {
    var select = document.createElement('select');
    select.setAttribute('class', 'form-control');
    for (var i = 0; i < roles.length; i++) {
        var option = document.createElement('option');
        option.value = roles[i].id;
        option.text = roles[i].name;
        select.appendChild(option);
    }
    return select;
}
function deleteFromUserArray(arr, login) {
    for (var i = 0; i < arr.length; i++) {
        if (arr[i] === login) {
            arr.splice(i, 1);
            break;
        }
    }

}
function createButton(fnc, txt) {
    var btn = document.createElement('button');
    var text = document.createTextNode(txt);
    btn.appendChild(text);
    btn.setAttribute('onclick', fnc);
    return btn;

}
function saveChanges(id) {
    var data = JSON.stringify({ AddedUsers: addedUsers, RemovedUsers: removedUsers, ProjectId: id });
    console.log(data);
    $.ajax({
        type: 'POST',
        url: "/User/ChangeParticipants", 
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function (result) {
            if (result !== null) {
                
                alert(result.result);
            } else {
                
                

            }
        }
    });

}