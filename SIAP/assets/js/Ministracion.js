document.getElementById('modal-form')
    .addEventListener('shown.bs.modal', function () {
        document.getElementById('txtnumministracion').focus();
    });
/**
 * @function cargarministraciones
 * @description Solicita al backend la lista principal de ministraciones para renderizar la tabla de datos.
 * Realiza una petición AJAX asíncrona enviando los filtros actuales (mes, fondo, folio).
 * Al recibir la respuesta (JSON), delega la construcción del HTML a la función _displaydatosministracion.
 * Atrapa errores HTTP 500 para evitar que la aplicación se congele silenciosamente.
 */
async function cargarministraciones() {
    try {
        const resp = await fetch("api/Ministraciones/ConsultaMinistraciones");
        const data = await resp.json();
        _displaydatosministracion(data);
    } catch (error) {
        console.error('No fue posible completar la solicitud', error);
    }
}
function _displaydatosministracion(data) {
    let html = "";
    data.forEach(item => {
        var importe = formatearMoneda(item.importe);
        var saldo = formatearMoneda(item.saldo);
        var fecha = formatearFecha(item.fecharegistro);
        var importeformateado = `<span class="spam6">${importe}</span>`;
        var opciones = `<button class="btn2 btn-icon " data-toggle="tooltip"  title="Modificar Ministración"  onclick="editarmini()"><img src="img/modificar.png"></button>`
                        + `<button class="btn2 btn-icon " data-toggle="tooltip"  title="Imprimir Ministración" ><img src="img/print.png"></button>`
                        + `<button class="btn2 btn-icon " data-toggle="tooltip"  title="Eliminar Ministración" ><img src="img/delete.png"></button>`;
        html += `<tr><td>${item.idministracion}</td>`;
        html += `<td><b>${item.numministracion}</b></td>`;
        html += `<td><b>${item.enlace}</b></td>`;
        html += `<td class="text-center">${item.tipoministracion}</td>`;
        html += `<td class="text-center">${importeformateado}</td>`;
        html += `<td class="text-center">${item.concepto}</td>`;
        html += `<td class="text-center">${saldo}</td>`;
        html += `<td class="text-center">${item.nombremes}</td></tr>`;
        html += `<td class="text-center">${fecha}</td></tr>`;
        html += `<td class="text-center">${opciones}</td></tr>`;
    })
    document.getElementById('display').innerHTML = html;
}
/**
 * @function Modalministracion
 * @description Función principal para invocar la ventana modal de Agregar/Editar Ministración.
 * Limpia los campos si es una inserción nueva, o se encarga de rellenarlos si es edición.
 * Esta función es típicamente llamada por el botón 'Agregar' en la interfaz de usuario.
 */
function Modalministracion() {
    $("#hddaccion").val("A|0");
    limpiar();
    combos();
    //$('#modal-form').modal({ backdrop: 'static', keyboard: false })
    //$("#modal-form").modal('show');
    //$("#modal-form").on('shown.bs.modal', function () {
        
    //});
    var modal = new bootstrap.Modal(document.getElementById('modal-form'), {
        backdrop: 'static',
        keyboard: false
    });
    modal.show();
}
function limpiar() {
    $("#txtnumministracion").val("");
    $("#txtfechasolicitud").val("");
    $("#txtconcepto").val("");
    $("#txtenlace").val("");
    $("#txtmontoministracion").val("");
}
function filtrar() {

}
//================================CATALOGOS=============================
async function catalogos() {
    removeOptions(document.getElementById('cboministracion'));
    removeOptions(document.getElementById('cbomes'));
    await catalogoministraciones();
    await catalogomes();
}
async function combos() {
    removeOptions(document.getElementById('cbotipoministracion'));
    await catalogotipoministracion();
    removeOptions(document.getElementById('cbomescaptura'));
    await catalogomescaptura();
}
//-------------Tipo de ministracion------------------------
async function catalogotipoministracion() {
    try {
        const resp = await fetch("api/Ministraciones/getTipoministracion");
        const data = await resp.json();
        _displaytipoministracion(data);
    } catch (error) {
        console.error('No fue posible completar la solicitud', error);
    }
}
function _displaytipoministracion(data) {
    const select = document.getElementById('cbotipoministracion');
    select.innerHTML = "<option value=''>Seleccione...</option>";  // LIMPIA

    data.forEach(item => {
        let option = document.createElement("option");
        option.textContent = item.tipoministracion;
        option.value = item.idtipoministracion;
        select.appendChild(option);
    });
}
//----------------Ministraciones --------------------------
async function catalogoministraciones() {
    try {
        const resp = await fetch("api/Ministraciones/getministraciones");
        const data = await resp.json();
        _displayministraciones(data);
    } catch (error) {
        console.error('No fue posible completar la solicitud', error);
    }
}
function _displayministraciones(data) {
    const select = document.getElementById('cboministracion');
    select.innerHTML = "<option value=''>Seleccione...</option>";  // LIMPIA

    data.forEach(item => {
        let option = document.createElement("option");
        option.textContent = item.numministracion;
        option.value = item.idministracion;
        select.appendChild(option);
    });
}
//--------------------mes ---------------------------------
async function catalogomes() {
    try {
        const resp = await fetch("api/Ministraciones/getmes");
        const data = await resp.json();
        _displaymes(data);
    } catch (error) {
        console.error('No fue posible completar la solicitud', error);
    }
}
function _displaymes(data) {
    const select = document.getElementById('cbomes');
    select.innerHTML = "<option value=''>Seleccione...</option>";  // LIMPIA
    data.forEach(item => {
        let option = document.createElement("option");
        option.textContent = item.nombremes;
        option.value = item.numeromes;
        select.appendChild(option);
    });
}
//----------------catalogo mes captura---------------------
async function catalogomescaptura() {
    try {
        const resp = await fetch("api/Ministraciones/getmes");
        const data = await resp.json();
        _displaymescaptura(data);
    } catch (error) {
        console.error('No fue posible completar la solicitud', error);
    }
}
function _displaymescaptura(data) {
    const select = document.getElementById('cbomescaptura');
    select.innerHTML = "<option value=''>Seleccione...</option>";  // LIMPIA
    data.forEach(item => {
        let option = document.createElement("option");
        option.textContent = item.nombremes;
        option.value = item.numeromes;
        select.appendChild(option);
    });
}
//================================CATALOGOS=============================
function removeOptions(selectElement) {
    var i, L = selectElement.options.length - 1;
    for (i = L; i >= 1; i--) {
        selectElement.remove(i);
    }
}
//---------------mascara numero--------------------------------------
function mascara(o, f) {
    v_obj = o;
    v_fun = f;
    setTimeout("execmascara()", 1);
}
function execmascara() {
    v_obj.value = v_fun(v_obj.value);
}
function cpf(v) {
    v = v.replace(/([^0-9\.]+)/g, '');
    v = v.replace(/^[\.]/, '');
    v = v.replace(/[\.][\.]/g, '');
    v = v.replace(/\.(\d)(\d)(\d)/g, '.$1$2');
    v = v.replace(/\.(\d{1,2})\./g, '.$1');
    v = v.toString().split('').reverse().join('').replace(/(\d{3})/g, '$1,');
    v = v.split('').reverse().join('').replace(/^[\,]/, '');
    return v;
}
function formatearMoneda(valor)
{
    var numero = parseFloat(valor);
    if (isNaN(numero) || numero === 0) {
        numero = 0;
    }
    return new Intl.NumberFormat('es-MX', {
        style: 'currency',
        currency: 'MXN'
    }).format(numero);
}
function formatearFecha(fecha) {

    if (!fecha || fecha === "0001-01-01T00:00:00") {
        return "";
    }

    return fecha.split('T')[0].split('-').reverse().join('-');
}
document.getElementById("txtnumministracion").addEventListener("input", function () {

    let valor = this.value.replace(/\D/g, "");

    let anioMes = valor.substring(0, 6);
    let consecutivo = valor.substring(6, 11);

    if (anioMes.length === 6) {
        this.value = anioMes + "-MM" + consecutivo;
    } else {
        this.value = anioMes;
    }

});
document.getElementById("txtenlace").addEventListener("input", function () {

    let valor = this.value.replace(/-/g, "").toUpperCase();

    let parte1 = valor.substring(0, 1);
    let parte2 = valor.substring(1, 2);
    let parte3 = valor.substring(2, 3);
    let parte4 = valor.substring(3, 5);
    let parte5 = valor.substring(5, 10);
    let parte6 = valor.substring(10, 11);
    let parte7 = valor.substring(11, 12);

    let resultado = "";

    if (parte1) resultado += parte1;
    if (parte2) resultado += "-" + parte2;
    if (parte3) resultado += "-" + parte3;
    if (parte4) resultado += "-" + parte4;
    if (parte5) resultado += "-" + parte5;
    if (parte6) resultado += "-" + parte6;
    if (parte7) resultado += "-" + parte7;

    this.value = resultado;

});
/**
 * @function btnmovimientosade
 * @description Disparador principal para guardar o editar el formulario de la Ministración.
 * Recopila los valores de los inputs (document.getElementById), construye un objeto JSON 
 * (payload) y hace el POST hacia /api/Ministraciones/CrearOActualizarMinistracion.
 * Utiliza SweetAlert2 para mostrar confirmaciones visuales al usuario.
 */
async function btnmovimientosade()
{
    try {
        const resp = await fetch("api/Ministraciones/ConsultaAdecuaciones");
        const data = await resp.json();
        _displaydatosclavespresupuestales(data);
    } catch (error) {
        console.error('No fue posible completar la solicitud', error);
    }
}
function _displaydatosclavespresupuestales(data) {
    let html = "";
    data.forEach(item => {
        var importe = formatearMoneda(item.importe);
        var saldo = formatearMoneda(item.saldo);
        var fecha = formatearFecha(item.fecharegistro);
        var importeformateado = `<span class="spam6">${importe}</span>`;
        var opciones = `<button class="btn2 btn-icon " data-toggle="tooltip"  title="Modificar Ministración"  onclick="editarmini()"><img src="img/modificar.png"></button>`
            + `<button class="btn2 btn-icon " data-toggle="tooltip"  title="Imprimir Ministración" ><img src="img/print.png"></button>`
            + `<button class="btn2 btn-icon " data-toggle="tooltip"  title="Eliminar Ministración" ><img src="img/delete.png"></button>`;
        html += `<tr><td>${item.idministracion}</td>`;
        html += `<td><b>${item.numministracion}</b></td>`;
        html += `<td><b>${item.enlace}</b></td>`;
        html += `<td class="text-center">${item.tipoministracion}</td>`;
        html += `<td class="text-center">${importeformateado}</td>`;
        html += `<td class="text-center">${item.concepto}</td>`;
        html += `<td class="text-center">${saldo}</td>`;
        html += `<td class="text-center">${item.nombremes}</td></tr>`;
        html += `<td class="text-center">${fecha}</td></tr>`;
        html += `<td class="text-center">${opciones}</td></tr>`;
    })
    document.getElementById('display').innerHTML = html;
}