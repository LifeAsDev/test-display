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
            loadWithRetry(elemento, uniqueUrl, 5, 700);
        } else if (["mp4", "webm", "ogg", "avi"].includes(ext)) {
            video = true;
            elemento = document.createElement("video");

            loadWithRetry(elemento, uniqueUrl, 5, 100);
            elemento.autoplay = false;
            elemento.muted = false;
            elemento.loop = true;
        } else {
            console.warn("Formato no soportado:", Url);
            return;
        }
    } else if (Texto) {        const uniqueUrl = Url + (Url.includes("?") ? "&" : "?") + "v=" + Date.now();

        // Crear elemento de texto
        elemento = document.createElement("div");
        //  elemento.textContent = Texto.Contenido || ""
        console.log(Texto.Contenido);
        elemento.innerHTML = Texto.Contenido || "";
        elemento.style.color = Texto.Color || "#fff";
        elemento.style.fontSize = (Texto.FontSize || 24) + "px";
        elemento.style.fontWeight = Texto.FontWeight || "normal";
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


    function mostrarElemento() {
        setTimeout(() => {
            elemento.style.opacity = (Opacidad / 100).toString();

            if (video) {
                elemento.play();
            }

            if (Texto) {
                elemento.dataset.originalHtml = Texto.Contenido || elemento.innerHTML;
                elemento.dataset.currentEffect = Texto.Efecto || 0;
                if (Texto.Efecto === 10) {
                    setTimeout(() => {
                        aplicarEfecto(elemento, Texto.Efecto || 0, Texto.Contenido);

                    }, FadeIn);
                } else {
                    aplicarEfecto(elemento, Texto.Efecto || 0);
                }
            }

            // FadeOut si corresponde
            if (FadeOut > 0) {
                setTimeout(() => {
                    elemento.style.transition = `opacity 0.1s ease-out`;
                    elemento.style.opacity = "0";
                }, FadeOut);
            }
        }, Math.max(Retraso, 10));
    }

    if (Url) {
        if (video) elemento.oncanplaythrough = () => mostrarElemento();
        else elemento.onload = () => mostrarElemento();
    } else mostrarElemento();

    container.appendChild(elemento);

    // Guardar en el mapa
    elementosMap.set(Id, { grupo: IdGrupo, nodo: elemento });
    elemento.dataset.originalHtml = Texto.Contenido || elemento.innerHTML;
    elemento.dataset.currentEffect = Texto.Efecto || 0;
}

function clearAllElements() {
    elementosMap.forEach(({ nodo }) => {
        if (nodo.parentNode) {
            nodo.parentNode.removeChild(nodo);
        }
    });
    elementosMap.clear();
}



function aplicarEfecto(elemento, efecto,innerHtml) {
    let inner;

    switch (efecto) {
        case 1: { // Máquina de escribir que respeta HTML
            const velocidad = 100; // ms por carácter (ajusta)
            const html = elemento.innerHTML;
            elemento.innerHTML = ""; // limpiamos el contenedor visual

            // parseamos el HTML original
            const temp = document.createElement("div");
            temp.innerHTML = html;

            const steps = [];

            // recorre el DOM original y crea la estructura (sin texto),
            // acumulando pasos para insertar los caracteres en los parents adecuados
            function traverse(node, parentTarget) {
                if (node.nodeType === Node.ELEMENT_NODE) {
                    // crear elemento sin hijos, copiar atributos
                    const el = document.createElement(node.tagName);
                    for (let i = 0; i < node.attributes.length; i++) {
                        const att = node.attributes[i];
                        el.setAttribute(att.name, att.value);
                    }
                    parentTarget.appendChild(el);

                    // recorrer hijos con este nuevo elemento como parent
                    Array.from(node.childNodes).forEach((child) => traverse(child, el));
                } else if (node.nodeType === Node.TEXT_NODE) {
                    const text = node.nodeValue || "";
                    // cada carácter es un paso que añade un TextNode al parentTarget
                    for (const ch of text) {
                        const char = ch; // captura por valor
                        steps.push(() => {
                            parentTarget.appendChild(document.createTextNode(char));
                        });
                    }
                }
                // comentarios u otros nodos los ignoramos
            }

            // construir estructura vacía y steps
            Array.from(temp.childNodes).forEach((child) => traverse(child, elemento));

            // ejecutar los pasos secuencialmente
            let idx = 0;
            function runStep() {
                if (idx >= steps.length) return;
                steps[idx]();
                idx++;
                // opcional: acelerar espacios
                const lastChar = (steps[idx - 1] && steps[idx - 1].toString()) || "";
                const nextDelay = (lastChar === " " ? Math.max(20, velocidad / 3) : velocidad);
                setTimeout(runStep, nextDelay);
            }
            runStep();
            break;
        }


        case 2: // Caer desde arriba
            elemento.style.position = "absolute";
            elemento.style.transform = "translateY(-50px)";
            setTimeout(() => {
                elemento.style.transition = "all 1s";
                elemento.style.transform = "translateY(0)";
            }, 100);
            break;

        case 3: // Desde izquierda
            elemento.style.position = "absolute";
            elemento.style.transform = "translateX(-50px)";
            setTimeout(() => {
                elemento.style.transition = "all 1s";
                elemento.style.transform = "translateX(0)";
            }, 50);
            break;

        case 4: // Desde derecha
            elemento.style.position = "absolute";
            elemento.style.transform = "translateX(50px)";
            setTimeout(() => {
                elemento.style.transition = "all 1s";
                elemento.style.transform = "translateX(0)";
            }, 50);
            break;
        case 5: // Caer desde arriba
            elemento.style.position = "absolute";
            elemento.style.transform = "translateY(50px)";
            setTimeout(() => {
                elemento.style.transition = "all 1s";
                elemento.style.transform = "translateY(0)";
            }, 50);
            break;
        case 6:
            elemento.style.overflow = "hidden";


             inner = document.createElement("div");
            while (elemento.firstChild) inner.appendChild(elemento.firstChild);
            elemento.appendChild(inner);

            inner.style.transform = "translateY(-100%)"; // empieza arriba

            setTimeout(() => {
                inner.style.transition = "transform 1.5s ease";
                inner.style.transform = "translateY(0)"; 
            }, 50);
            break;

        case 7:
            elemento.style.overflow = "hidden";
         

             inner = document.createElement("div");
            while (elemento.firstChild) inner.appendChild(elemento.firstChild);
            elemento.appendChild(inner);

            inner.style.transform = "translateY(100%)"; // empieza abajo

            setTimeout(() => {
                inner.style.transition = "transform 1.5s ease";
                inner.style.transform = "translateY(0)"; 
            }, 50);
            break;

        case 8:
            transformarDivASlashed(elemento); break;

        case 9:
            elemento.classList.add("warningEffect"); break;
        case 10:
            elemento.classList.add("smokemonster");

            // Tomar el HTML real en lugar de textContent
            const originalHTML = elemento.innerHTML;
            elemento.innerHTML = "";

            // Parsear HTML para mantener los spans originales
            const temp = document.createElement("div");
            temp.innerHTML = originalHTML;

            function procesarNodo(node, parentTarget) {
                if (node.nodeType === Node.ELEMENT_NODE) {
                    // Mantener el elemento (ej: <span style="...">)
                    const el = document.createElement(node.tagName);
                    for (let i = 0; i < node.attributes.length; i++) {
                        const att = node.attributes[i];
                        el.setAttribute(att.name, att.value);
                    }
                    parentTarget.appendChild(el);

                    // Procesar recursivamente los hijos de este nodo
                    Array.from(node.childNodes).forEach((child) =>
                        procesarNodo(child, el)
                    );
                } else if (node.nodeType === Node.TEXT_NODE) {
                    const text = node.nodeValue || "";
                    text.split("").forEach((letra, i) => {
                        const span = document.createElement("span");
                        span.textContent = letra;
                        span.style.setProperty("--i", i); // índice dinámico
                        parentTarget.appendChild(span);
                    });
                }
            }

            Array.from(temp.childNodes).forEach((child) =>
                procesarNodo(child, elemento)
            );

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



function setVideoBucle(id, valor) {
    const obj = elementosMap.get(id);
    if (obj && obj.nodo && obj.nodo.tagName === "VIDEO") {
        obj.nodo.loop = valor; // true = con loop, false = sin loop
        if (valor) obj.nodo.play().catch(err => {
            console.warn("No se pudo reproducir el video:", err);
        });
    }
}

function cambiaOpacidad(id, valor) {
    const obj = elementosMap.get(id);
    if (obj && obj.nodo) {
        obj.nodo.style.opacity = (valor / 100).toString();
     
    }
}

function ocultaObjeto(id) {
    const obj = elementosMap.get(id);
    if (obj && obj.nodo) {
        obj.nodo.style.display = "none";
    }
}

function mostrarObjeto(id) {
    const obj = elementosMap.get(id);
    if (obj && obj.nodo) {
        obj.nodo.style.display = "";
    }
}

function eliminaObjeto(id) {
    const obj = elementosMap.get(id);
    if (obj && obj.nodo) {
        obj.nodo.remove();
        elementosMap.delete(id); // limpiar del mapa también
    }
}

function loadWithRetry(elemento, url, maxRetries = 3, delay = 1000) {
    let attempts = 0;

    function tryLoad() {
        attempts++;
        const uniqueUrl = url + (url.includes("?") ? "&" : "?") + "v=" + Date.now();
        elemento.src = uniqueUrl;

        elemento.onerror = () => {
            if (attempts < maxRetries) {
                console.warn(`Fallo al cargar (${attempts}), reintentando...`);
                setTimeout(tryLoad, delay);
            } else {
                console.error(`No se pudo cargar después de ${maxRetries} intentos: ${url}`);
            }
        };
    }

    tryLoad();
}

function editarTexto(id, opciones) {
    const obj = elementosMap.get(id);
    if (!obj || !obj.nodo) {
        console.warn("No existe el objeto con id:", id);
        return;
    }
    const elemento = obj.nodo;
    if (!(elemento instanceof HTMLElement) || elemento.tagName !== "DIV") {
        console.warn("El objeto no parece ser un elemento de texto (DIV):", id);
        return;
    }

    // destructurar opciones con defaults seguros
    const {
        Contenido,
        Color,
        FontSize,
        FontWeight,
        FontFamily,
        Align,
        Efecto,
        ForzarReaplicar = false,
        ResetEffect = false
    } = opciones || {};

    // ------------- estilos / alineado (no tocan estructura de efecto) -------------
    if (Color !== undefined) elemento.style.color = Color;
    if (FontSize !== undefined) elemento.style.fontSize = FontSize + "px";
    if (FontWeight !== undefined) elemento.style.fontWeight = FontWeight;
    if (FontFamily !== undefined) elemento.style.fontFamily = FontFamily;
    if (Align === "center") {
        elemento.style.left = elemento.style.left || elemento.dataset.posX + "px";
        elemento.style.transform = "translateX(-50%)";
    } else if (Align === "right") {
        elemento.style.left = elemento.style.left || elemento.dataset.posX + "px";
        elemento.style.transform = "translateX(-100%)";
    } else if (Align !== undefined) {
        elemento.style.transform = "";
    }
    // ---------------------------------------------------------------------------

    // helpers para remover clases de efectos conocidos
    const efectoClases = ["smokemonster", "fantasma", "slashed", "warningEffect"];
    function removeEffectClasses(el) {
        efectoClases.forEach(c => el.classList.remove(c));
    }


    // Si cambian el contenido:
    if (Contenido !== undefined) {
        // actualizar snapshot
        elemento.dataset.originalHtml = Contenido;

        const curEf = parseInt(elemento.dataset.currentEffect || "0", 10) || 0;

        if (Efecto === undefined) {
            // sin efecto -> simple replace
            elemento.innerHTML = Contenido;
            elemento.dataset.currentEffect = 0;
        } else {
            // hay un efecto -> siempre restaurar y reaplicar
            removeEffectClasses(elemento);
            elemento.innerHTML = Contenido;
            elemento.dataset.currentEffect = Efecto;
            aplicarEfecto(elemento, Efecto, Contenido);
        }

    }

}
