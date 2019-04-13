var formElements = [];
var optionValues = [];
var isTextarea = true;

function createColumnObjectForSavingToDatabase(headrName, valueType, optionType) {

    var obj = new Object();
    obj.ColumnName = headrName;
    obj.Type = valueType;
    if (optionType !== null) {
        obj.Option = optionType;
    }
    //formElements.push(obj);
    return obj;
}

function addColumn() {
    if (updateState !== undefined) {
        updateReviewForm();
    }
    else {
        var tab = document.getElementById('firstTableRow');
        var th = document.createElement('th');
        var columnName = document.getElementById('columnName');
        var text = document.createTextNode(columnName.value);
        var deleteButton = document.createElement('button');
        var spn = document.createElement('span');
        var x = document.createTextNode('x');
        spn.appendChild(x);
        deleteButton.appendChild(spn);
        deleteButton.type = 'button';
        deleteButton.setAttribute('class', 'close');
        deleteButton.setAttribute('onclick', 'removeColumnFromTable(this)');
        th.setAttribute('ondblclick', 'restoreOptionData(this)');
        th.appendChild(text);
        th.appendChild(deleteButton);
        tab.appendChild(th);
        if (isTextarea === true) {
            var name = columnName.value;
            var element = createColumnObjectForSavingToDatabase(name, 'textarea', null);
            console.log(element);
            formElements.push(element);
        }
        else {
            var name = columnName.value;
            var element = createColumnObjectForSavingToDatabase(name, 'select', optionValues);
            console.log(element);
            formElements.push(element);
        }
        createRowTypeInTabel();

        columnName.value = "";
        optionValues = [];
        console.log(formElements);
    }

}

function createRowTypeInTabel() {
    var tableColumnsCount = document.getElementById('workSpaceTable').rows.length;
    //console.log(tableColumnsCount);
    if (tableColumnsCount === 1) {
        //var text = document.createTextNode('text in td');
        var text = getElementType();

        var table = document.getElementById('workSpaceTable');
        var row = document.createElement('tr');
        var td = document.createElement('td');
        td.appendChild(text);

        row.appendChild(td);
        table.appendChild(row);

        //removeElement();
    }
    else {
        var table = document.getElementById('workSpaceTable').rows[1];
        //var text = document.createTextNode('text in td');
        var text = getElementType();
        var td = document.createElement('td');
        td.appendChild(text);
        table.appendChild(td);
    }

}
/**
 * Return typ of table column for defined form
 */
function getElementType() {
    var e = document.getElementById('columnType');
    var type = e.options[e.selectedIndex].value;
    if (type === 'opt') {
        var select = document.createElement('select');
        select.setAttribute('class', 'form-control')
        var optionValue = getValuesFromOptionSetup();
        for (var i = 0; i < optionValue.length; i++) {
            var options = document.createElement('option');
            options.value = optionValue[i];
            options.text = optionValue[i];
            select.appendChild(options);
            optionValues.push(optionValue[i]);
        }
        removeElement();

        return select;
    }
    else if (type === 'text') {
        var area = document.createElement('textarea');
        area.setAttribute('class', 'form-control');
        area.style = "resize: both;"
        return area;
    }
}


/**
 * Shows setup window of option menu creator
 */
function showSetupWindow() {

    var setupWindow = document.getElementById("setupWindow");
    var visibility = document.getElementById('columnType');
    var table = document.getElementById('optionSetup');
    var type = visibility.options[visibility.selectedIndex].value;
    if (type === 'opt') {
        setupWindow.style.visibility = "visible";
        table.style.display = "table";
        isTextarea = false;
    }
    else if (type === 'text') {
        setupWindow.style.visibility = "hidden";
        table.style.display = "none";
        isTextarea = true;
    }

}
var formValue = 0;
function createInputText(values) {
    var textInput = document.createElement('input');
    textInput.type = 'text';
    textInput.id = 'formValue' + formValue;
    textInput.setAttribute('class', 'form-control');
    if (values !== undefined)
        textInput.value = values;
    var element = document.getElementById('optionSetup');
    var tr = document.createElement('tr');
    var td = document.createElement('td');
    var text = document.createTextNode('Option value:');
    td.appendChild(text);
    var td1 = document.createElement('td');
    var td2 = document.createElement('td');
    var btn = document.createElement('input');
    btn.type = 'button';
    btn.value = 'delete';
    btn.setAttribute('onclick', 'removeRowFromTable(this)');
    btn.setAttribute('class', 'btn btn-secondary btn-sm');
    //btn.onclick = removeRowFromTable(this);
    td2.appendChild(btn);
    td1.appendChild(textInput);
    tr.appendChild(td);
    tr.appendChild(td1);
    tr.appendChild(td2);
    element.appendChild(tr);
    formValue++;
}

function getValuesFromOptionSetup() {
    var optionValues = [];
    var inputs = document.getElementById('optionForm').getElementsByTagName('input');
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type != 'text') {
            continue;
        }
        optionValues.push(inputs[i].value);
    }
    return optionValues;
}
/**
 * Removes element from option creator menu
 */
function removeElement() {
    document.getElementById('columnName').value = "";
    var table = document.getElementById('optionSetup');
    var tableRows = table.rows.length;
    for (var i = 0; i < tableRows; i++) {
        table.deleteRow(0);

    }
    formValue = 0;
    updateState = undefined;
}
/**
 * Remove row from table which contains selected option element
 * @param {concrete index of row in table} index 
 */
function removeRowFromTable(index) {
    var i = index.parentNode.parentNode.rowIndex;
    document.getElementById("optionSetup").deleteRow(i);
}

function removeColumnFromTable(index) {
    var table = document.getElementById('workSpaceTable');
    index = index.parentNode.cellIndex;
    formElements.slice(index, index);//remove element from specific index in array
    console.log(index);
    for (var i = 0; i < table.rows.length; i++) {
        table.rows[i].deleteCell(index);
    }
}

function restoreOptionData(index) {
    removeElement();
    var table = document.getElementById('workSpaceTable');
    var name = table.rows[0].cells[index.cellIndex].childNodes[0].nodeValue;
    var option = table.rows[1].cells[index.cellIndex].childNodes[0];
    if (option.type === 'textarea') {
        createSetupWindow(name, 'text');
    }
    else {
        var arr = []
        for (var i = 0; i < option.length; i++) {
            arr.push(option[i].value);
        }
        createSetupWindow(name, option, arr);
    }
    updateState = index.cellIndex;

}
function createSetupWindow(name, type, options) {
    formValue = 0;
    var columnName = document.getElementById('columnName');
    var opt = document.getElementById('columnType');
    columnName.value = name;
    if (type === 'text') {
        opt.value = 'text';
        showSetupWindow();
    }
    else {
        optionValues = [];
        console.log("optionz");
        for (var i = 0; i < options.length; i++) {
            createInputText(options[i]);
            optionValues.push(options[i]);
            formValue++;
        }
        opt.value = 'opt';
        showSetupWindow();
    }
}
var updateState = undefined;
function updateReviewForm() {
    if (updateState !== undefined) {
        var table = document.getElementById('workSpaceTable');
        table.rows[0].cells[updateState].childNodes[0].nodeValue = document.getElementById('columnName').value;
        var option = table.rows[1].cells[updateState].childNodes[0];
        option.remove();
        option = table.rows[1].cells[updateState];
        option.appendChild(getElementType());
        updateState = undefined;
    }
}


function postReviewTameplate() {
    //console.log(JSON.stringify({ model : formElements }));
    var roles = saveAllRoles();
    var tameplateName = document.getElementById('tameplateName').value;
    var tameplateDescription = document.getElementById('tameplateDescription').value;
    var e = document.getElementById('columnType');
    var type = e.options[e.selectedIndex].value;
    var dta = JSON.stringify({ model: formElements, Name: tameplateName, Descritpion: tameplateDescription, header: headerElements, role: roles });
    console.log(dta);
    $.ajax({
        type: 'POST',
        url: "http://localhost:49727/Tameplate/SaveTameplate", // http://localhost:60000/api/upload/ -- na tuto URL se budou posilat diagramy (XML)
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: dta,
        success: function (result) {
            if (result !== null) {
                // document.getElementById("result").innerHTML = JSON.stringify(result);
                alert(result.result);
            } else {
                //document.getElementById("result").innerHTML = "NULL";
                alert("KO");

            }
        }
    });
}