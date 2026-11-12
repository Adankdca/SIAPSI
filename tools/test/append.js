
$(document).ready(function() {
    $('#modalCargarOficio').on('hidden.bs.modal', function () {
        document.getElementById("fileOficio").value = "";
        let fileNameEl = document.getElementById("fileOficioName");
        if(fileNameEl) {
            fileNameEl.innerText = 'o haz clic para explorar (.pdf, .xls, .xlsx)';
            fileNameEl.classList.remove('text-primary', 'font-weight-bold');
            fileNameEl.closest('.modern-file-label').style.backgroundColor = '#f8f9fa';
        }
        $("#uploadResult").html("");
        $("#btnProcesarOficio").prop("disabled", false).text("Procesar Archivo");
    });
});
