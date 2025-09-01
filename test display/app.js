// Un Map global para guardar los elementos creados
const elementosMap = new Map();

function saludar(url) {
    const container = document.body;

    // Aseguramos que el contenedor tenga posición relativa
    container.style.position = "relative";

    // Obtener la extensión del archivo
    const ext = url.split(".").pop().toLowerCase();

    let elemento;

    if (["png", "jpg", "jpeg", "gif", "webp"].includes(ext)) {
        // Es una imagen
        elemento = document.createElement("img");
        elemento.src = url;
    } else if (["mp4", "webm", "ogg", "avi"].includes(ext)) {
        // Es un video
        elemento = document.createElement("video");
        elemento.src = url;
        elemento.autoplay = true;
        elemento.muted = true;
        elemento.loop = true;
    } else {
        console.warn("Formato no soportado:", url);
        return;
    }

    // Estilos comunes
    elemento.style.position = "absolute";
    elemento.style.top = Math.floor(Math.random() * 80) + "%";   // posición aleatoria
    elemento.style.left = Math.floor(Math.random() * 80) + "%";  // posición aleatoria


    // Agregar al contenedor
    container.appendChild(elemento);

    // Guardar en el Map usando la url como key
    elementosMap.set(url, elemento);
}
