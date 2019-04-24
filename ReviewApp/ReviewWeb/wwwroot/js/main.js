function postReviewTameplate(){
    var name = document.getElementById('templateName').value;
    var description = document.getElementById('templateDescription').value;
    var formElements = getReviewFormValues();
    var roles = saveAllRoles();
    var headers = getAllCustomRows();
    var data = JSON.stringify({ model: formElements, Name: name, Description: description, role: roles, header: headers });
    console.log(data);
    $.ajax({
        type: 'POST',
        url: "http://localhost:49727/Template/SaveTemplate", // http://localhost:60000/api/upload/ -- na tuto URL se budou posilat diagramy (XML)
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: data,
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
/**************ROLES HANDLING*****************************************/
function addRowToRoleTable() {
    var table = document.getElementById('roleTable');
    var row = document.createElement('tr');
    var input = document.createElement('input');
    var td = document.createElement('td');
    input.type = "text";
    input.setAttribute('class', 'form-control');
    var roleName = document.createTextNode('Role name: ');
    td.appendChild(roleName);
    var td1 = document.createElement('td');
    td1.appendChild(input);
    var btn = document.createElement('button');
    var removeText = document.createTextNode('Remove');

    btn.appendChild(removeText);
    btn.setAttribute('onclick', 'removeRole(this);');
    btn.setAttribute('class', 'btn btn-primary btn-sm');
    var td2 = document.createElement('td');
    td2.appendChild(btn);
    row.appendChild(td);
    row.appendChild(td1);
    row.appendChild(td2);
    table.appendChild(row);
}
function removeRole(index) {
    var removeIndex = index.parentNode.parentNode.rowIndex;
    var table = document.getElementById('roleTable');
    table.deleteRow(removeIndex);
}
function saveAllRoles() {
    var inputs = document.getElementById('roleTable').getElementsByTagName('input');
    var roles = [];
    for (var i = 0; i < inputs.length; i++) {
        roles.push(inputs[i].value);
    }
    return roles;
}
/*****************************************************************************/
/************************FORM HANDLING************************************** */
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
function createInputText(values) {
    var textInput = document.createElement('input');
    textInput.type = 'text';
    //textInput.id = 'formValue' + formValue;
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
    //formValue++;
}
function removeRowFromTable(index) {
    var i = index.parentNode.parentNode.rowIndex;
    document.getElementById("optionSetup").deleteRow(i);
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
        var btn = document.getElementById('addColumnBtn');
        btn.value = "Add column";
        /*if (isTextarea === true) {
            var name = columnName.value;
            var element = createColumnObjectForSavingToDatabase(name, 'textarea', null);
            console.log(element);
            formElements.push(element);
        }
        else {
            //var name = columnName.value;
            //var element = createColumnObjectForSavingToDatabase(name, 'select', optionValues);
            //console.log(element);
            //formElements.push(element);
        }*/
        createRowTypeInTabel();

        columnName.value = "";
        //optionValues = [];
        //console.log(formElements);
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
            //optionValues.push(optionValue[i]);
        }
        removeElement();

        return select;
    }
    else if (type === 'text') {
        var area = document.createElement('textarea');
        area.setAttribute('class', 'form-control');
        area.style = "resize: both;"
        removeElement();
        return area;
    }
}
var updateState = undefined;
function removeElement() {
    document.getElementById('columnName').value = "";
    var table = document.getElementById('optionSetup');
    var tableRows = table.rows.length;
    for (var i = 0; i < tableRows; i++) {
        table.deleteRow(0);

    }
    formValue = 0;
    updateState = undefined;
    var btn = document.getElementById('addColumnBtn');
        btn.value = "Add column";

}
function getValuesFromOptionSetup() {
    var optionValues = [];
    var inputs = document.getElementById('optionForm').getElementsByTagName('input');
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type !== 'text') {
            continue;
        }
        optionValues.push(inputs[i].value);
    }
    return optionValues;
}
function restoreOptionData(index) {
    removeElement();
    var table = document.getElementById('workSpaceTable');
    var name = table.rows[0].cells[index.cellIndex].childNodes[0].nodeValue;
    var option = table.rows[1].cells[index.cellIndex].childNodes[0];
    document.getElementById('addColumnBtn').value = 'Change column';
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
//var updateState = undefined;
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
function removeColumnFromTable(index) {
    var table = document.getElementById('workSpaceTable');
    index = index.parentNode.cellIndex;
    //formElements.slice(index, index);//remove element from specific index in array
    console.log(index);
    for (var i = 0; i < table.rows.length; i++) {
        table.rows[i].deleteCell(index);
    }
}
function getReviewFormValues(){
    var table = document.getElementById('workSpaceTable');
    var formElements = [];
    for(var i = 0; i < table.rows[0].cells.length; i++){
        var element = getColumn(table,i);
        formElements.push(element);
    }
    return formElements;
}
function getColumn(table,index){
    console.log(table.rows[0].cells[index]);
    var headerCellName = table.rows[0].cells[index].textContent;
    console.log(headerCellName);
    headerCellName = headerCellName.substr(0,headerCellName.length - 1);
    var contentCell = table.rows[1].cells[index];
    var select = contentCell.getElementsByTagName('select');
    //if select length is equal 0 than the cell contains textarea
    if(select.length === 0)
        return createColumnObjectForSavingToDatabase(headerCellName,'textarea',null);
    else{
        var optionValues = getOptionElements(select[0]);
        return createColumnObjectForSavingToDatabase(headerCellName,'select',optionValues);
    }

}
function getOptionElements(select){
    var optionValues = [];
    for(var i = 0; i < select.length; i++){
        var optvalue = select.options[i].value;
        optionValues.push(optvalue);
    }
    return optionValues;
}
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


/********************************************************************************************** */
/***************************HEADER CONFIG****************************************************** */
function showSpecificationOfRow() {
    var specWindow = document.getElementById('headerSpecification');
    specWindow.style.visibility = 'visible';
}
function hideSpecificationOfRow() {
    var specWindow = document.getElementById('headerSpecification');
    specWindow.style.visibility = 'hidden';
}
function showColumns() {
    var atr = document.getElementById('Columns');
    var tr = document.createElement('tr');
    var td = document.createElement('td');
    var text = document.createTextNode('Column: ');
    td.appendChild(text);
    tr.appendChild(td);
    var td1 = document.createElement('td');
    //console.log(formElements);
    var formElements = getReviewFormValues();
    var select = createSelect(formElements);
    if (select === null) {
        var btn = document.getElementById('saveButton');
        btn.setAttribute('hidden', '');
        return;
    }
    td1.appendChild(select);
    tr.appendChild(td1);
    atr.appendChild(tr);
}
function removeColumns() {
    var tbody = document.getElementById('Columns');
    var rows = tbody.rows.length;
    for (var i = 0; i < rows; i++) {
        tbody.deleteRow(0);
    }
    var btn = document.getElementById('saveButton');
    btn.removeAttribute('hidden');
}
function createSelect(option) {

    var select = document.createElement('select');
    select.id = 'specificColumn';
    select.setAttribute('onchange', 'showOptionValues();');
    if (option.length === 0)
        return null;
    else {
        select.appendChild(createOption(''));
        for (var i = 0; i < option.length; i++) {
            if (option[i].Type === 'select') {
                var opt = createOption(option[i].ColumnName);
                opt.value = option[i].ColumnName;
                select.appendChild(opt);
            }
        }
        return select;
    }
}
function createOption(name) {
    var opt = document.createElement('option');
    var text = document.createTextNode(name);
    opt.value = name;
    opt.appendChild(text);
    return opt;
}
function showOptionValues() {
    var formElements = getReviewFormValues();
    var specificColumnValue = document.getElementById('specificColumn').value;
    var slect = document.createElement('select');
    slect.id = 'columnEnums';
    console.log(formElements[0].Option);
    for (var i = 0; i < formElements.length; i++) {
        if (formElements[i].ColumnName === specificColumnValue) {
            var options = formElements[i].Option;
            for (var j = 0; j < options.length; j++) {
                var opt = createOption(options[j]);
                slect.appendChild(opt);
            }
        }
    }
    var table = document.getElementById('Columns');
    var tr = document.createElement('tr');
    var td = document.createElement('td');
    var text = document.createTextNode('Column value');
    td.appendChild(text);
    var td1 = document.createElement('td');
    td1.appendChild(slect);
    tr.appendChild(td);
    tr.appendChild(td1);
    table.appendChild(tr);

}
function appendToHeaderTable() {
    var table = document.getElementById('CustomValue');
    var tr = document.createElement('tr');
    var td = document.createElement('td');
    var atributName = document.getElementById('CustomValueName').value;
    var text = document.createTextNode(atributName);
    td.appendChild(text);
    var td1 = document.createElement('td');
    var td2 = document.createElement('td');
    var btn = document.createElement('button');
    //var btnName = document.createTextNode('Delete');
    var rmv = document.createTextNode('Remove');
    btn.appendChild(rmv);
    btn.setAttribute('onclick', 'removeRowFromHeaderTable(this);');
    btn.setAttribute('class', 'btn btn-primary btn-sm');
    td2.appendChild(btn);
    if (document.getElementById('noneRadio').checked === true) {
        var inp = document.createElement('input');
        inp.type = 'text';
        td1.appendChild(inp);
        //createHeaderElement(atributName, null, null);
    }
    else {
        var specificColumnValue = document.getElementById('specificColumn').value;
        var specificColumnEnumValue = document.getElementById('columnEnums').value;
        //var txt = document.createTextNode('sum for column: ' + specificColumnValue + ' option: ' + specificColumnEnumValue);
        //td1.appendChild(txt);
        td1.innerHTML = "<span id=\"fnc\">sum</span> for column: <span id=\"columnName\">"+specificColumnValue+"</span> option: <span id=\"opt\">"+specificColumnEnumValue+"</span>";
        console.log(specificColumnValue);
        //createHeaderElement(atributName, 'sum', specificColumnEnumValue, specificColumnValue);
        removeColumns();
    }
    tr.appendChild(td);
    tr.appendChild(td1);
    tr.appendChild(td2);
    table.appendChild(tr);
    atributName = document.getElementById('CustomValueName');
    atributName.value = "";
    document.getElementById('noneRadio').checked = true;
    hideSpecificationOfRow();

}
function removeRowFromHeaderTable(index) {
    // od  rowIndexu odecitam pocet radku, ktere jsou pevne definované v tabulce
    var row = index.parentNode.parentNode.rowIndex - 6;
    var table = document.getElementById('CustomValue');
    table.deleteRow(row);
}
function getAllCustomRows(){
    var headeElements = [];
    var table = document.getElementById('CustomValue');
    for(var i = 0; i < table.rows.length; i++){
        var columnName = table.rows[i].cells[0].innerText;
        var rowType = table.rows[i].cells[1].getElementsByTagName('input');
        if(rowType.length > 0){
            var element = createHeaderElement(columnName,null,null,null);
            headeElements.push(element);
        }
        else{
            var tmp = table.rows[i].cells[1].getElementsByTagName('span');
            console.log("test fnc");
            console.log(tmp);
            /*
            var fnc = document.getElementById('fnc').innerText;
            var parameter = document.getElementById('opt').innerText;
            var column = document.getElementById('columnName').innerText;*/
            var fnc = tmp.fnc.innerText;
            var parameter = tmp.opt.innerText;
            var column = tmp.columnName.innerText;
            var element = createHeaderElement(columnName,fnc,parameter,column);
            headeElements.push(element);
        }
    }
    return headeElements;
}
function createHeaderElement(name, fcn, parameter,columnName) {
    var element = new Object();
    element.name = name;
    if (fcn !== null) {
        element.fcn = fcn;
        element.parameter = parameter;
        console.log(columnName);
        element.columnName = columnName;
    }
    //headerElements.push(element);
    return element;
}
