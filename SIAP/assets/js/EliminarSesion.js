
function eliminarsesion() {
    Swal.fire({
            title: "Cerrar Sesión",
            text: "¿Está Seguro de salir?",
            type: 'warning',
            showCancelButton: true,
            confirmButtonText: "Sí",
            cancelButtonText: "Cancelar",
        })
        .then(resultado => {
            if (resultado.value) {
                // Hicieron click en "Sí"
                sessionStorage.clear();
                window.location.href = 'Login.aspx';
            } else {

            }
        });
}