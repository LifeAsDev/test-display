// Mapa global para manejar los grupos y objetos
const elementosMap = new Map();

function agregarObjetoDisplay(config) {
    console.log(config);
    const {
        IdGrupo,
        Id,
        Url,
        Ancho = 200,
        Alto = 200,
        PosX = 0,
        PosY = 0,
        NivelCapa = 0,
        Opacidad = 100,
        Retraso = 0,
        FadeIn = 0,
        FadeOut = 0
    } = config;

    const container = document.getElementById("image-container") || document.body;

    container.style.position = "relative";
    container.style.width = "100vw";
    container.style.height = "100vh";
    container.style.overflow = "hidden";

    // Crear el elemento según tipo
    const ext = Url.split(".").pop().toLowerCase();
    let elemento;

    if (["png", "jpg", "jpeg", "gif", "webp"].includes(ext)) {
        elemento = document.createElement("img");
        elemento.src = Url;
    } else if (["mp4", "webm", "ogg", "avi"].includes(ext)) {
        elemento = document.createElement("video");
        elemento.src = Url;
        elemento.autoplay = true;
        elemento.muted = false;
        elemento.loop = true;
    } else {
        console.warn("Formato no soportado:", Url);
        return;
    }

    // Estilos iniciales
    elemento.id = Id;
    elemento.dataset.grupo = IdGrupo;
    elemento.style.position = "absolute";
    elemento.style.left = PosX + "px";
    elemento.style.top = PosY + "px";
    // Width
    if (Ancho > 0) {
        elemento.style.width = Ancho + "px";
    } else {
        elemento.style.width = "auto";
    }

    // Height
    if (Alto > 0) {
        elemento.style.height = Alto + "px";
    } else {
        elemento.style.height = "auto";
    }

    elemento.style.zIndex = NivelCapa;
    elemento.style.opacity = "0"; // inicia invisible
    elemento.style.transition = `opacity ${FadeIn}ms ease-in`;

    // Retraso para mostrar
    setTimeout(() => {
        elemento.style.opacity = (Opacidad / 100).toString();

        // FadeOut si corresponde
        if (FadeOut > 0) {
            setTimeout(() => {
                elemento.style.transition = `opacity ${FadeOut}ms ease-out`;
                elemento.style.opacity = "0";
            }, 3000); // aquí puedes usar duración personalizada si tienes "tiempo total de vida"
        }
    }, Retraso);

    container.appendChild(elemento);

    // Guardar en el mapa
    elementosMap.set(Id, { grupo: IdGrupo, nodo: elemento });
}
