
const canvas = document.getElementById("displayCanvas");
const ctx = canvas.getContext("2d");

const elementosMap = new Map();

function agregarObjetoDisplay(config) {
    const { Id, Url, Ancho = 200, Alto = 200, PosX = 0, PosY = 0, Opacidad = 100 } = config;

    const ext = Url.split(".").pop().toLowerCase();
    if (!["mp4","webm","ogg"].includes(ext)) return;

    const vid = document.createElement("video");
    vid.src = Url;
    vid.muted = true;
    vid.autoplay = true;
    vid.playsInline = true;

    const objeto = {
        id: Id,
        x: PosX,
        y: PosY,
        w: Ancho,
        h: Alto,
        opacity: Opacidad / 100,
        recurso: vid
    };

    vid.addEventListener("loadeddata", () => {
        // Si ancho o alto eran 0, usar tamaño natural del video
        if (objeto.w === 0) objeto.w = vid.videoWidth;
        if (objeto.h === 0) objeto.h = vid.videoHeight;
        // --- Solución para el bucle preciso ---
        vid.addEventListener("timeupdate", () => {
            // Reiniciar el video justo antes del final
            // El valor de 0.05 segundos es una buena práctica, pero puedes ajustarlo
            if (vid.currentTime >= vid.duration - 0.05) {
                vid.currentTime = 0;
            }
        });
        elementosMap.set(Id, objeto);
        vid.play().catch(() => { });
    });
}

function drawAll() {
    console.log(elementosMap);
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    for (const el of elementosMap.values()) {
    
        if (el.recurso.readyState >= 2) {
       
            ctx.globalAlpha = el.opacity;
            ctx.drawImage(el.recurso, el.x, el.y, el.w, el.h);
        }
    }
    ctx.globalAlpha = 1;
    requestAnimationFrame(drawAll);
}



drawAll();
