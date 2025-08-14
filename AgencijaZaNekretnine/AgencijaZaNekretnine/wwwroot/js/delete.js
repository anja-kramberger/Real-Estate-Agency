function Delete(url) {
    swal({
        title: "Kliknite OK ukoliko zelite da obrisete",
        text: "Klikom na OK podaci ce trajno biti obrisani",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(reloadPage, 3000);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    });
}

function reloadPage() {
    window.location.reload();
}