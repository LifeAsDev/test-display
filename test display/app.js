// Mapa global para manejar los grupos y objetos
const elementosMap = new Map();

function agregarObjetoDisplay(config) {
    console.log(config);
    const {
        IdGrupo,
        Id,
        Url = "",
        Texto = null, // objeto con configuraciones de texto
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

    let elemento;
    let video = false;
    if (Url) {
        // Crear el elemento según tipo de archivo
        const ext = Url.split(".").pop().toLowerCase();

        if (["png", "jpg", "jpeg", "gif", "webp"].includes(ext)) {
            elemento = document.createElement("img");
            elemento.src = Url;
        } else if (["mp4", "webm", "ogg", "avi"].includes(ext)) {
            video = true;
            elemento = document.createElement("video");
            elemento.src = Url;
            elemento.autoplay = false;
            elemento.muted = false;
            elemento.loop = true;
        } else {
            console.warn("Formato no soportado:", Url);
            return;
        }
    } else if (Texto) {
        // Crear elemento de texto
        elemento = document.createElement("div");
        elemento.textContent = Texto.Contenido || "";
       // elemento.style.color = Texto.Color || "#fff";
        elemento.style.fontSize = (Texto.FontSize || 24) + "px";
     //   elemento.style.fontWeight = Texto.FontWeight || "normal";
        elemento.style.fontFamily = Texto.FontFamily || "sans-serif";
        elemento.style.whiteSpace = "pre-wrap"; // para soportar saltos de línea
        if (Texto.Align === "center") {
            elemento.style.left = PosX + "px";
            elemento.style.transform = "translateX(-50%)";
        } else if (Texto.Align === "right") {
            elemento.style.left = PosX + "px";
            elemento.style.transform = "translateX(-100%)"; 
        } else {
            elemento.style.left = PosX + "px";
        }
    } else {
        console.warn("Ni Url ni Texto definidos para el objeto:", Id);
        return;
    }

    // Estilos generales
    elemento.id = Id;
    elemento.dataset.grupo = IdGrupo;
    elemento.style.position = "absolute";
    elemento.style.left = PosX + "px";
    elemento.style.top = PosY + "px";

    // Width
    if (Ancho > 0 && !Texto) {
        elemento.style.width = Ancho + "px";
    } else {
        elemento.style.width = "auto";
    }

    // Height
    if (Alto > 0 && !Texto) {
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
        if (video) {
            elemento.play();
        }
        // FadeOut si corresponde
        if (FadeOut > 0) {
            setTimeout(() => {
                elemento.style.transition = `opacity ${FadeOut}ms ease-out`;
                elemento.style.opacity = "0";
            }, 3000); // TODO: podrías reemplazar este 3000 por un parámetro "Duracion"
        }
    }, Retraso);

    container.appendChild(elemento);

    // Guardar en el mapa
    elementosMap.set(Id, { grupo: IdGrupo, nodo: elemento });
}
