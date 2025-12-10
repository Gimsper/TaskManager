import Swal from "sweetalert2";

export function loader() {
    return Swal.fire({
        background: 'none',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
}

export function closeLoader() {
    Swal.close();
}