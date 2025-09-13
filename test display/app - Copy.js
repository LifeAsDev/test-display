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
        FadeOut = 0,
        ObjectFit = "contain"
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
        const uniqueUrl = Url + (Url.includes("?") ? "&" : "?") + "v=" + Date.now();

        if (["png", "jpg", "jpeg", "gif", "webp"].includes(ext)) {
            elemento = document.createElement("img");
            elemento.src = uniqueUrl;
        } else if (["mp4", "webm", "ogg", "avi"].includes(ext)) {
            video = true;
            elemento = document.createElement("video");
            elemento.src = uniqueUrl;
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
        //  elemento.textContent = Texto.Contenido || ""
        elemento.innerHTML = Texto.Contenido || "";
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
    elemento.style.objectFit = ObjectFit
    elemento.style.zIndex = NivelCapa;
    elemento.style.opacity = "0"; // inicia invisible
    elemento.style.transition = (elemento.style.transition ? elemento.style.transition + ', ' : '') + `opacity ${FadeIn}ms ease-in`;


    // Retraso para mostrar
    setTimeout(() => {
        elemento.style.opacity = (Opacidad / 100).toString();
        if (video) {
            elemento.play();
        }
        if (Texto) {
            if (Texto.Efecto === 10) {
                setTimeout(() => {
                    aplicarEfecto(elemento, Texto.Efecto || 0);
                }, FadeIn)
            }
            else {
                aplicarEfecto(elemento, Texto.Efecto || 0);
            }
        }
     
        // FadeOut si corresponde
        if (FadeOut > 0) {
            setTimeout(() => {
                elemento.style.transition = `opacity ${.1}s ease-out`;
                elemento.style.opacity = "0";
            }, FadeOut); 
        }
    },Math.max( Retraso,10));

    container.appendChild(elemento);

    // Guardar en el mapa
    elementosMap.set(Id, { grupo: IdGrupo, nodo: elemento });
}

function clearAllElements() {
    elementosMap.forEach(({ nodo }) => {
        if (nodo.parentNode) {
            nodo.parentNode.removeChild(nodo);
        }
    });
    elementosMap.clear();
}



function aplicarEfecto(elemento, efecto) {
    let inner;

    switch (efecto) {
        case 1: // Máquina de escribir
            const texto = elemento.textContent;
            elemento.textContent = "";
            let i = 0;
            function maquina() {
                if (i < texto.length) {
                    elemento.textContent += texto[i];
                    i++;
                    setTimeout(maquina, 100);
                }
            }
            maquina();
            break;

        case 2: // Caer desde arriba
            elemento.style.position = "absolute";
            elemento.style.transform = "translateY(-50px)";
            elemento.style.transition = "all 1s";
            setTimeout(() => {
                elemento.style.transform = "translateY(0)";
            }, 50);
            break;

        case 3: // Desde izquierda
            elemento.style.position = "absolute";
            elemento.style.transform = "translateX(-50px)";
            elemento.style.transition = "all 1s";
            setTimeout(() => {
                elemento.style.transform = "translateX(0)";
            }, 50);
            break;

        case 4: // Desde derecha
            elemento.style.position = "absolute";
            elemento.style.transform = "translateX(50px)";
            elemento.style.transition = "all 1s";
            setTimeout(() => {
                elemento.style.transform = "translateX(0)";
            }, 50);
            break;
        case 5: // Caer desde arriba
            elemento.style.position = "absolute";
            elemento.style.transform = "translateY(50px)";
            elemento.style.transition = "all 1s";
            setTimeout(() => {
                elemento.style.transform = "translateY(0)";
            }, 50);
            break;
        case 6:
            elemento.style.overflow = "hidden";


             inner = document.createElement("div");
            while (elemento.firstChild) inner.appendChild(elemento.firstChild);
            elemento.appendChild(inner);

            inner.style.transform = "translateY(-100%)"; // empieza arriba
            inner.style.transition = "transform 1.5s ease";

            setTimeout(() => {
                inner.style.transform = "translateY(0)"; 
            }, 50);
            break;

        case 7:
            elemento.style.overflow = "hidden";
         

             inner = document.createElement("div");
            while (elemento.firstChild) inner.appendChild(elemento.firstChild);
            elemento.appendChild(inner);

            inner.style.transform = "translateY(100%)"; // empieza abajo
            inner.style.transition = "transform 1.5s ease";

            setTimeout(() => {
                inner.style.transform = "translateY(0)"; 
            }, 50);
            break;

        case 8:
            transformarDivASlashed(elemento); break;

        case 9:
            elemento.classList.add("warningEffect"); break;
        case 10:
            elemento.classList.add("smokemonster");
            const smokeText = elemento.textContent;
            elemento.textContent = "";

            smokeText.split("").forEach((letra, i) => {
                const span = document.createElement("span");
                span.textContent = letra;
                span.style.setProperty("--i", i); // índice dinámico
                elemento.appendChild(span);
            });
            break;
        case 11:
            elemento.classList.add("fantasma");
            elemento.setAttribute("data-text", elemento.textContent);
            break;


    }
}

function transformarDivASlashed(element) {
    if (!(element instanceof HTMLElement)) return;

    // Texto original
    const texto = element.textContent.trim();

    // Limpio el contenido
    element.textContent = "";

    // Agrego clase "slashed" manteniendo las demás
    element.classList.add("slashed");

    // Crear hijos
    const top = document.createElement("div");
    top.className = "top";
    top.setAttribute("title", texto);

    const bot = document.createElement("div");
    bot.className = "bot";
    bot.setAttribute("title", texto);

    // Insertar dentro del elemento original
    element.appendChild(top);
    element.appendChild(bot);

    return element;
}
