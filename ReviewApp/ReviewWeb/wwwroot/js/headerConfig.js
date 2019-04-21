var customAtributes = [];

var headerElements = [];

function createHeaderElement(name, fcn, parameter,columnName) {
    var element = new Object();
    element.name = name;
    if (fcn !== null) {
        element.fcn = fcn;
        element.parameter = parameter;
        console.log(columnName);
        element.columnName = columnName;
    }
    headerElements.push(element);
}

function showColumns() {
    var atr = document.getElementById('Columns');
    var tr = document.createElement('tr');
    var td = document.createElement('td');
    var text = document.createTextNode('Column: ');
    td.appendChild(text);
    tr.appendChild(td);
    var td1 = document.createElement('td');
    console.log(formElements);
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
function removeColumns() {
    var tbody = document.getElementById('Columns');
    var rows = tbody.rows.length;
    for (var i = 0; i < rows; i++) {
        tbody.deleteRow(0);
    }
    var btn = document.getElementById('saveButton');
    btn.removeAttribute('hidden');
}

function showOptionValues() {
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
        createHeaderElement(atributName, null, null);
    }
    else {
        var specificColumnValue = document.getElementById('specificColumn').value;
        var specificColumnEnumValue = document.getElementById('columnEnums').value;
        var txt = document.createTextNode('sum for column: ' + specificColumnValue + ' option: ' + specificColumnEnumValue);
        td1.appendChild(txt);
        console.log(specificColumnValue);
        createHeaderElement(atributName, 'sum', specificColumnEnumValue, specificColumnValue);
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
function showSpecificationOfRow() {
    var specWindow = document.getElementById('headerSpecification');
    specWindow.style.visibility = 'visible';
}
function hideSpecificationOfRow() {
    var specWindow = document.getElementById('headerSpecification');
    specWindow.style.visibility = 'hidden';
}
/******************************/
/******ROLE CONFIGURATOR******/
/****************************/

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