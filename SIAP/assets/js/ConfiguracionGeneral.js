const url = "api/ConfiguracionGeneral";
const url2 = "api/Firmantes";
const url3 = "api/UpFirmantes";
const url4 = "api/DelFirmantes";
const url5 = "api/UpConfig";


function addConfiguracion() {
    fetch(url)
        .then(response => response.json())
        .then(data => _displayconf(data))
        .catch(error => console.error('No fué posible completar la solicitud.', error));
}

function _displayconf(data) {
    let html = "";
    let dep = "";
    let clave = "";
    let titular = "";
    let tableconf = "";
    let id = 1;
    let guardconfig = "";

    data.forEach(item => {
        html = `<b>Año de Ejercicio</b>: ${item.Anio_ejercicio}`;
        dep = `<b>Dependencia</b>: ${item.Nombre_dependencia}`;
        clave = `${item.Clave}`;
        titular = `${item.Titular}`;

        tableconf += `<tr>`;
        tableconf += `<td class="text-center">${id++}</td>`;
        tableconf += `<td>${item.Nombre_dependencia}</td>`;
        tableconf += `<td>${item.Clave}</td>`;
        tableconf += `<td><input type="text" style="background-color:transparent" class="form-control sinborde" id="nt${item.Idconfiguracion}" value="${item.Titular}" disabled></td>`;
        tableconf += `<td class="text-center">${item.Anio_ejercicio}</td>`;
        tableconf += `<td class="text-center">`;
        tableconf += `<button class="btn2 btn-icon" onclick="editarconf(${item.Idconfiguracion})"><img src="img/modificar.png"></button>`;
        //tableconf += `<button class="btn2 btn-icon" onclick="eliminarconf()"><img src="img/delete.png"></button>`;
        tableconf += `</td>`;
        tableconf += `</tr>`;
        guardconfig += `<button class="btn btn-dark linea" style="display:none" onclick="guardarconfig(${item.Idconfiguracion}, 1)" id="guardar1">Guardar</button>`;
        guardconfig += `<button class="btn btn-danger linea" style="display:none" onclick="guardarconfig(${item.Idconfiguracion}, 2)" id="cancelar1">Cancelar</button>`;
    })
    document.getElementById('anio').innerHTML = html;
    document.getElementById('dependencia').innerHTML = dep;
    document.getElementById('clave').innerHTML = clave;
    document.getElementById('titular').innerHTML = titular;
    document.getElementById('contenidoconf').innerHTML = tableconf;
    document.getElementById('gconfig').innerHTML = guardconfig;

    addFirmantes();
}

function addFirmantes() {
    fetch(url2)
        .then(response => response.json())
        .then(data => _displayfirm(data))
        .catch(error => console.error('No fué posible completar la solicitud.', error));
}

function _displayfirm(data) {
    let firmantes = "";
    let tablefirm = "";
    let guardfirm = "";
    let id = 1;

    data.forEach(item => {
        firmantes += `<div class="col-3">`;
        firmantes += `   <p class="text-uppercase text-sm"><b>FIrmante:</b><br /><b>Cargo:</b></p>`;
        firmantes += `</div>`;
        firmantes += `<div class="col-8">`;
        firmantes += `   <p class="text-uppercase text-sm">${item.Nombre} <br>${item.Cargo}</p>`;
        firmantes += `</div>`;
        tablefirm += `<tr>`;
        tablefirm += `<td class="text-center">${id++}</td>`;
        tablefirm += `<td><input type="text" style="background-color:transparent" class="form-control sinborde" id="nf${item.Idfirmante}" value="${item.Nombre}" disabled></td>`;
        tablefirm += `<td><input type="text" style="background-color:transparent" class="form-control sinborde" id="cf${item.Idfirmante}" value="${item.Cargo}" disabled></td>`;
        tablefirm += `<td class="text-center">`;
        tablefirm += `<button class="btn2 btn-icon" onclick="editarfirm(${item.Idfirmante})"><img src="img/modificar.png"></button>`;
        tablefirm += `<button class="btn2 btn-icon" onclick="eliminarfirm(${item.Idfirmante})"><img src="img/delete.png"></button>`;
        tablefirm += `</td>`;
        tablefirm += `</tr>`;
        guardfirm += `<button class="btn btn-dark linea" style="display:none" onclick="guardarfirm(${item.Idfirmante}, 1)" id="${item.Idfirmante}guardar">Guardar</button>`;
        guardfirm += `<button class="btn btn-danger linea" style="display:none" onclick="guardarfirm(${item.Idfirmante}, 2)" id="${item.Idfirmante}cancelar">Cancelar</button>`;
    })
    document.getElementById('firmantes').innerHTML = firmantes;
    document.getElementById('firmantesconf').innerHTML = tablefirm;
    document.getElementById('gfirm').innerHTML = guardfirm;
}

function actualizarConf() {

    $("#modal-form").modal("show");
}

function editarfirm(nf) {
    var tb = document.getElementById(`nf${nf}`);
    tb.disabled = false;
    tb.focus();
    tb.className = "form-control";
    var tb1 = document.getElementById(`cf${nf}`);
    tb1.disabled = false;
    tb1.className = "form-control";
    var btn = document.getElementById(`${nf}guardar`);
    btn.style.display = "block";
    var btn = document.getElementById(`${nf}cancelar`);
    btn.style.display = "block";
}

function guardarfirm(nf, tipo) {
    if (tipo == 1) {
        const item = {
            nombre: document.getElementById(`nf${nf}`).value.trim(),
            cargo: document.getElementById(`cf${nf}`).value.trim()
        };

        fetch(`${url3}/${nf}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
            .then(() => {
                Swal.fire({
                    //title: "Cerrar Sesión",
                    text: "Datos guardados correctamente.",
                    type: 'success',
                    timer: 3000
                })
                addConfiguracion();

            })
    } else if (tipo == 2) {
        var tb = document.getElementById(`nf${nf}`);
        tb.disabled = true;
        tb.className = "form-control sinborde";
        var tb1 = document.getElementById(`cf${nf}`);
        tb1.disabled = true;
        tb1.className = "form-control sinborde";
        var btn = document.getElementById(`${nf}guardar`);
        btn.style.display = "none";
        var btn = document.getElementById(`${nf}cancelar`);
        btn.style.display = "none";
    }
   
}

function eliminarfirm(id) {
    Swal.fire({
        title: "Eliminar",
        text: "¿Está Seguro de eliminar el registro?",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: "Sí",
        cancelButtonText: "Cancelar",
    })
        .then(resultado => {
            if (resultado.value) {
                // Hicieron click en "Sí"
                Swal.fire({
                    //title: "Cerrar Sesión",
                    text: "Registro eliminado.",
                    type: 'success',
                    timer: 3000
                })
                fetch(`${url4}/${id}`, {
                    method: 'DELETE'
                }).then(() => {
                    addConfiguracion();
                })
                
            } else {
                addConfiguracion();
            }
        });

}

function editarconf(nt) {
    var tb2 = document.getElementById(`nt${nt}`);
    tb2.disabled = false;
    tb2.focus();
    tb2.className = "form-control";

    var btn = document.getElementById('guardar1');
    btn.style.display = "block";
    var btn = document.getElementById('cancelar1');
    btn.style.display = "block";
}

function guardarconfig(nt, tipo) {
    if (tipo == 1) {
        const item = {
            Titular: document.getElementById(`nt${nt}`).value.trim(),
        };
        fetch(`${url5}/${nt}`, {
            method: 'PUT',
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(item)
        })
            .then(() => {
                Swal.fire({
                    //title: "Cerrar Sesión",
                    text: "Datos guardados correctamente.",
                    type: 'success',
                    timer: 3000
                })
                addConfiguracion();

            })
    }
    else if (tipo == 2) {
        var tb = document.getElementById(`nt${nt}`);
        tb.disabled = true;
        tb.className = "form-control sinborde";
        var btn = document.getElementById('guardar1');
        btn.style.display = "none";
        var btn = document.getElementById('cancelar1');
        btn.style.display = "none";
    }
}