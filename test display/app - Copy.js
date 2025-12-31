
const elementosMap = new Map();

function agregarObjetoDisplay(config) {
    console.log(config);

    let {
        IdGrupo,
        Id,
        Url = "",
        Ancho = 0,
        Alto = 0,
        PosX = 0,
        PosY = 0,
        NivelCapa = 0,
        Opacidad = 100,
        Retraso = 0,
        FadeIn = 0,
        FadeOut = 0,
        RetrasoOut = 0,
        ObjectFit = "contain",
        Replace = false,
        Mute = false,
        LoopVideo = false,
        CierrateAlAcabar,
        Rotacion = 0,
        VoltearHorizontal = false,
        VoltearVertical = false
    } = config;

    const container = document.getElementById("image-container") || document.body;

    container.style.position = "relative";
    container.style.width = "100vw";
    container.style.height = "100vh";
    container.style.overflow = "hidden";

    let video = false;
    let elemento;

    // =====================================================
    // 🧨 1. REPLACE: eliminar el elemento existente
    // =====================================================
    let viejo = elementosMap.get(Id)?.nodo;

    if (Replace === true && viejo) {
        viejo.remove();
        viejo = null;
        elementosMap.delete(Id);
    }

    // =====================================================
    // ⚡ 2. Si NO hay elemento existente → crear uno nuevo
    // =====================================================
    const isDataUrl = Url.startsWith("data:");
    console.log(isDataUrl);

    if (!viejo) {
        if (Url) {
            // Detectar cámara
            const match = Url.match(/^camera(\d+)$/);

            if (match) {
                const camIndex = parseInt(match[1], 10);
                video = true;

                elemento = document.createElement("video");
                elemento.autoplay = true;
                elemento.muted = true;
                elemento.playsInline = true;

                navigator.mediaDevices
                    .enumerateDevices()
                    .then((devices) => {
                        const videoDevices = devices.filter((d) => d.kind === "videoinput");

                        if (!videoDevices[camIndex]) {
                            console.warn("No existe esa cámara, señor. Tomaré la cámara 0…");
                        }

                        const deviceId =
                            videoDevices[camIndex]?.deviceId || videoDevices[0]?.deviceId;

                        return navigator.mediaDevices.getUserMedia({
                            video: { deviceId },
                            audio: false,
                        });
                    })
                    .then((stream) => {
                        elemento.srcObject = stream;
                    })
                    .catch((err) => {
                        console.error("Error accediendo a la cámara:", err);
                    });
            }
            else if (isDataUrl) {

                // =========================
                // 🧠 DATA URL (Base64)
                // =========================
                elemento = document.createElement("img");
                elemento.src = Url;
            } else {
                // Imagen o video normal
                const ext = Url.split(".").pop().toLowerCase();
                // Fuerza a que la URL tenga formato file:///
                const fileUrl = Url.startsWith("file://")
                    ? Url
                    : "file:///" + Url.replace(/\\/g, "/");

                // Agregar el parámetro único
                const uniqueUrl =
                    fileUrl + (fileUrl.includes("?") ? "&" : "?") + "v=" + Date.now();

                if (["png", "jpg", "jpeg", "gif", "webp"].includes(ext)) {
                    elemento = document.createElement("img");
                    loadWithRetry(elemento, uniqueUrl, 5, 700);
                } else if (["mp4", "webm", "ogg", "avi", "m4v"].includes(ext)) {
                    video = true;
                    elemento = document.createElement("video");

                    loadWithRetry(elemento, uniqueUrl, 5, 100);
                    elemento.autoplay = false;
                    elemento.muted = Mute;
                    elemento.loop = LoopVideo;
                    if (CierrateAlAcabar) {
                        elemento.addEventListener("ended", () => {
                            elemento.style.visibility = "hidden";
                            // si prefieres quitarlo del flujo:
                            // elemento.style.display = "none";
                        });
                    }
                } else {
                    console.warn("Formato no soportado:", Url);
                    return;
                }
            }
        }


    }
    else {
        // =====================================================
        // 🛠 3. NO Replace: reutilizar el elemento existente
        // =====================================================
        elemento = viejo;
        Retraso = 0;

        if (!Url) return;

        let resolvedUrl = Url;

        try {
            // Soporta ./ , ../ , \ y file:// correctamente
            resolvedUrl = new URL(Url.replace(/\\/g, "/"), document.baseURI).href;
        } catch (e) {
            resolvedUrl = Url;
        }

        // =====================================
        // 🎥 VIDEO: cambio controlado de src
        // =====================================
        if (elemento.tagName === "VIDEO") {

            if (elemento.src !== resolvedUrl) {
                const wasPlaying = !elemento.paused;
                const t = elemento.currentTime;

                elemento.src = resolvedUrl;
                elemento.load();

                elemento.addEventListener(
                    "loadedmetadata",
                    () => {
                        try {
                            elemento.currentTime = t;
                            if (wasPlaying) elemento.play();
                        } catch { }
                    },
                    { once: true }
                );
            }

        }
        // =====================================
        // 🖼️ NO VIDEO: cache-buster normal
        // =====================================
        else {

            if (isDataUrl) {
                elemento.src = Url;
            }

            else if (elemento.src !== resolvedUrl) {
                const uniqueUrl =
                    resolvedUrl +
                    (resolvedUrl.includes("?") ? "&" : "?") +
                    "v=" + Date.now();

                elemento.src = uniqueUrl;
            }
        }
    }

    elemento.style.opacity = "0";
    elemento.style.transition =
        (elemento.style.transition ? elemento.style.transition + ", " : "") +
        `opacity ${FadeIn}ms ease-in`;
    // =====================================================
    // 🎨 Estilos generales
    // =====================================================

    elemento.id = Id;
    elemento.dataset.grupoId = IdGrupo;

    elemento.style.position = "absolute";
    elemento.style.left = PosX + "px";
    elemento.style.top = PosY + "px";

    elemento.style.width = Ancho > 0 ? Ancho + "px" : "auto";
    elemento.style.height = Alto > 0 ? Alto + "px" : "auto";

    elemento.style.objectFit = ObjectFit;
    elemento.style.zIndex = NivelCapa;
    const scaleX = VoltearHorizontal ? -1 : 1;
    const scaleY = VoltearVertical ? -1 : 1;

    elemento.style.transform = `
        scale(${scaleX}, ${scaleY})
        rotate(${Rotacion}deg)
    `;
    // Fade-in
    elemento.dataset.fadeOut = FadeOut;

    // =====================================================
    // 👁 Mostrar elemento (fade / retraso / autoplay)
    // =====================================================
    function mostrarElemento() {
        elemento._timerMostrar = setTimeout(() => {
            elemento.style.opacity = (Opacidad / 100).toString();
            if (video) elemento.play();

            if (RetrasoOut > 0) {
                setTimeout(() => {
                    elemento.style.transition = `opacity ${FadeOut}ms ease-out`;
                    elemento.style.opacity = "0";
                    setTimeout(() => {

                        if (video) elemento.pause();
                    }, FadeOut);
                }, RetrasoOut);
            }
        }, Retraso);
    }

    if (Url) {
        if (video) elemento.oncanplaythrough = () => mostrarElemento();
        else elemento.onload = () => mostrarElemento();
    } else mostrarElemento();

    // =====================================================
    // 🧩 Agregar al DOM (solo si es nuevo)
    // =====================================================
    if (!viejo) {
        container.appendChild(elemento);
    }


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

function aplicarEfecto(elemento, efecto, innerHtml) {
    let inner;

    switch (efecto) {
        case 1: {
            // Máquina de escribir que respeta HTML
            const velocidad = 100;
            const html = elemento.innerHTML;
            elemento.innerHTML = "";

            // --- cancelamos ejecuciones anteriores ---
            if (elemento._typingTimer) {
                console.log(elemento._typingTimer);
                console.log(html);
                clearTimeout(elemento._typingTimer);
                elemento._typingTimer = null;
            }
            const runId = Symbol("typing");
            elemento._typingRunId = runId;

            const temp = document.createElement("div");
            temp.innerHTML = html;
            const steps = [];

            function traverse(node, parentTarget) {
                if (node.nodeType === Node.ELEMENT_NODE) {
                    const el = document.createElement(node.tagName);
                    for (let i = 0; i < node.attributes.length; i++) {
                        const att = node.attributes[i];
                        el.setAttribute(att.name, att.value);
                    }
                    parentTarget.appendChild(el);
                    Array.from(node.childNodes).forEach((child) => traverse(child, el));
                } else if (node.nodeType === Node.TEXT_NODE) {
                    const text = node.nodeValue || "";
                    for (const ch of text) {
                        steps.push(() => {
                            parentTarget.appendChild(document.createTextNode(ch));
                        });
                    }
                }
            }
            Array.from(temp.childNodes).forEach((child) => traverse(child, elemento));

            let idx = 0;
            function runStep() {
                // si ya hubo otra edición, cancelamos
                if (elemento._typingRunId !== runId) return;

                if (idx >= steps.length) return;
                steps[idx]();
                idx++;

                // ⚠️ aquí corrige tu lógica de espacios
                const char = html[idx - 1];
                const nextDelay =
                    char === " " ? Math.max(20, velocidad / 3) : velocidad;

                elemento._typingTimer = setTimeout(runStep, nextDelay);
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
            transformarDivASlashed(elemento);
            break;

        case 9:
            elemento.classList.add("warningEffect");
            break;
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

        case 11: {
            elemento.classList.add("anaglyph");
            break;
        }




        case 12: {
            // ticker vertical paso a paso

            elemento.style.overflow = "hidden";

            // ⛔ No pisar absolute
            if (!elemento.style.position || elemento.style.position === "") {
                elemento.style.position = "relative";
            }
            // Guardar width actual, si existía
            const widthAnterior = elemento.style.width;
            // limpiar contenido
            const text = (innerHtml || elemento.innerHTML).replace(/\\n/g, "\n");
            elemento.innerHTML = "";

            // dividir en items
            const items = text.split(/\r?\n/).filter((t) => t.trim() !== "");

            // contenedor interno
            const inner = document.createElement("div");
            inner.style.position = "absolute";
            inner.style.top = "0";
            inner.style.left = "0";
            inner.style.transition = "transform 0.5s ease";
            elemento.appendChild(inner);

            // agregar items
            items.forEach((txt) => {
                const div = document.createElement("div");
                div.innerHTML = txt;
                inner.appendChild(div);
            });

            // medir altura del primer item
            requestAnimationFrame(() => {
                const itemHeight = inner.firstElementChild.offsetHeight;
                elemento.style.height = itemHeight + "px";
                // ⚡ Si el width del elemento es 0, lo calculamos desde el primer item
                if (
                    !widthAnterior ||
                    widthAnterior === "" ||
                    elemento.offsetWidth === 0
                ) {
                    const autoWidth = inner.firstElementChild.offsetWidth;
                    elemento.style.width = autoWidth + "px";
                }

                let index = 0;
                setInterval(() => {
                    index = (index + 1) % items.length;
                    inner.style.transform = `translateY(-${index * itemHeight}px)`;
                }, 1500);
            });

            break;
        }
        case 13: {
            const nodes = Array.from(elemento.childNodes);
            elemento.innerHTML = "";

            let letterIndex = 0;

            nodes.forEach(node => {
                // Si es un <br>, se conserva tal cual
                if (node.nodeType === Node.ELEMENT_NODE && node.tagName === "BR") {
                    elemento.appendChild(document.createElement("br"));
                    return;
                }

                // Si es texto, dividir en letras
                if (node.nodeType === Node.TEXT_NODE) {
                    [...node.textContent].forEach(ch => {
                        const span = document.createElement("span");
                        span.textContent = ch;
                        span.style.display = "inline-block";
                        span.style.animation = "bounce 0.6s ease infinite";
                        span.style.animationDelay = `-${letterIndex * 0.1}s`;
                        span.style.animationFillMode = "both";
                        elemento.appendChild(span);
                        letterIndex++;
                    });
                }
            });

            break;
        }

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
        if (valor)
            obj.nodo.play().catch((err) => {
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

function ocultaGrupo(idGrupo) {
    for (const obj of elementosMap.values()) {
        if (obj.grupo === idGrupo && obj.nodo) {
            obj.nodo.style.display = "none";
        }
    }
}

function mostrarGrupo(idGrupo) {
    for (const obj of elementosMap.values()) {
        if (obj.grupo === idGrupo && obj.nodo) {
            obj.nodo.style.display = "";
        }
    }
}



function eliminaObjeto(id, retraso = 0, fadeOut = 0) {
    const obj = elementosMap.get(id);
    if (!obj || !obj.nodo) return;

    const nodo = obj.nodo;
    const fade =
        fadeOut === -1
            ? Number(nodo.dataset.fadeOut ?? 0)
            : fadeOut;

    setTimeout(() => {
        // Asegurar estado inicial
        nodo.style.opacity = getComputedStyle(nodo).opacity;
        nodo.style.transition = `opacity ${fade}ms ease-out`;

        requestAnimationFrame(() => {
            nodo.style.opacity = "0";
        });

        // Eliminar cuando termina el fade
        setTimeout(() => {
            if (nodo.tagName === "VIDEO") {
                nodo.pause();
                nodo.removeAttribute("src");
                nodo.srcObject = null; // Liberar el stream si es necesario
            }
            nodo.remove();
            elementosMap.delete(id);
        }, fade);

    }, retraso);
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
                console.error(
                    `No se pudo cargar después de ${maxRetries} intentos: ${url}`
                );
            }
        };
    }

    tryLoad();
}

function agregarTexto(id, opciones, replace) {
    let obj = elementosMap.get(id);
    console.log(replace);
    if (replace === true && obj && obj.nodo) {
        obj.nodo.remove();
        elementosMap.delete(id);
        obj = null;
    }

    // ------------------------------------------------------
    // ⚡ Crear si no existe
    // ------------------------------------------------------
    if (!obj || !obj.nodo) {
        console.warn("No existe el objeto con id, creando:", id);

        const div = document.createElement("div");
        div.id = id;

        div.style.position = "absolute";

        div.style.left = (opciones?.PosX ?? 0) + "px";
        div.style.top = (opciones?.PosY ?? 0) + "px";

        div.dataset.posX = opciones?.PosX ?? 0;
        div.dataset.posY = opciones?.PosY ?? 0;

        div.style.zIndex = opciones?.NivelCapa ?? 0;
        div.style.opacity = ((opciones?.Opacidad ?? 100) / 100).toString();

        // IMPORTANT → activar transición para fade
        div.style.transition = "opacity 0.5s ease";

        // Agregar propiedad grupoId
        div.dataset.grupoId = opciones?.GrupoId ?? "";

        obj = { nodo: div };
        elementosMap.set(id, obj);

        const contenedor =
            document.getElementById("contenedorPrincipal") || document.body;
        contenedor.appendChild(div);
    }
    // ------------------------------------------------------

    const elemento = obj.nodo;

    if (!(elemento instanceof HTMLElement) || elemento.tagName !== "DIV") {
        console.warn("El objeto no parece ser un elemento de texto (DIV):", id);
        return;
    }

    let {
        Contenido,
        Color,
        FontSize,
        FontWeight,
        FontFamily,
        Align,
        Efecto,
        Ancho,
        Alto,
        PosX,
        PosY,
        NivelCapa,
        Opacidad,

        // 👇 NUEVAS PROPIEDADES
        FadeIn = 0,
        FadeOut = 0,
        RetrasoIn = 0,
        RetrasoOut = 0,

        ForzarReaplicar = false,
        ResetEffect = false,
        Rotacion,
        Mayusculas,
        Minusculas,
        Sombra,
        TextAlign,
        WhiteSpace
    } = opciones || {};

    // ----------------------
    // ✨ aplicar nuevas props
    // ----------------------
    let changed = false;

    if (PosX !== undefined) {
        elemento.style.left = PosX + "px";
        elemento.dataset.posX = PosX;
        changed = true;
    }

    if (PosY !== undefined) {
        elemento.style.top = PosY + "px";
        elemento.dataset.posY = PosY;
        changed = true;
    }

    if (Ancho !== undefined && Ancho > 0) {
        elemento.style.width = Ancho + "px";
        changed = true;
    }

    if (Alto !== undefined && Alto > 0) {
        elemento.style.height = Alto + "px";
        changed = true;
    }

    if (!elemento._borderTimer) {
        elemento._borderTimer = null;
    }


    if (NivelCapa !== undefined) elemento.style.zIndex = NivelCapa;

    if (Opacidad !== undefined) {
        elemento.style.opacity = (Opacidad / 100).toString();
    }

    if (Color !== undefined) elemento.style.color = Color;
    if (FontSize !== undefined) elemento.style.fontSize = FontSize + "px";
    if (FontWeight !== undefined) elemento.style.fontWeight = FontWeight;
    if (FontFamily !== undefined) elemento.style.fontFamily = FontFamily;
    if (WhiteSpace !== undefined) elemento.style.whiteSpace = WhiteSpace;

    if (Align === "center") {
        if (FontFamily !== undefined) elemento.style.fontFamily = FontFamily;

        elemento.style.left = elemento.dataset.posX + "px";
        elemento.style.transform = "translateX(-50%)";
    } else if (Align === "right") {
        elemento.style.left = elemento.dataset.posX + "px";
        elemento.style.transform = "translateX(-100%)";
    } else if (Align !== undefined) {
        elemento.style.transform = "";
    }
    if (TextAlign !== undefined) {
        elemento.style.textAlign = TextAlign;
    }

    // helpers efectos
    const efectoClases = ["smokemonster", "fantasma", "slashed", "warningEffect"];
    function removeEffectClasses(el) {
        efectoClases.forEach((c) => el.classList.remove(c));
    }
    if (Rotacion !== undefined) {
        let actual = elemento.style.transform || "";
        actual = actual.replace(/rotate\([^)]*\)/, "").trim();
        elemento.style.transform = `${actual} rotate(${Rotacion}deg)`.trim();
    }

    let contenidoProcesado = Contenido;

    if (Contenido !== undefined) {
        if (Mayusculas === true) contenidoProcesado = Contenido.toUpperCase();
        if (Minusculas === true) contenidoProcesado = Contenido.toLowerCase();
    }

    if (Contenido !== undefined) {
        elemento.dataset.originalHtml = contenidoProcesado;

        if (elemento._timerMostrar) clearTimeout(elemento._timerMostrar);

        if (Efecto === undefined) {
            elemento.innerHTML = contenidoProcesado;
            elemento.dataset.currentEffect = 0;
        } else {
            removeEffectClasses(elemento);
            elemento.innerHTML = contenidoProcesado;
            elemento.dataset.currentEffect = Efecto;
            aplicarEfecto(elemento, Efecto, contenidoProcesado);
        }
    }
    if (Sombra !== undefined) {
        // ejemplo: "2px 2px 5px red"
        elemento.style.textShadow = Sombra;
    }
    // ---------------------------------------------------
    // 🌑 FADE IN / FADE OUT
    // ---------------------------------------------------

    // asegurar transición correcta:
    elemento.style.transition = `opacity ${FadeIn}ms ease`;
    // START FADE IN
    if (RetrasoIn > 0 || FadeIn > 0) {
        elemento.style.opacity = 0;
        setTimeout(() => {
            console.log(Opacidad / 100);

            elemento.style.opacity = (Opacidad ?? 100) / 100;
        }, RetrasoIn);
    }

    elemento.dataset.fadeOut = FadeOut;

    // START FADE OUT
    if (RetrasoOut > 0) {
        setTimeout(() => {
            elemento.style.transition = `opacity ${FadeOut}ms ease`;
            elemento.style.opacity = 0;
        }, RetrasoOut + RetrasoIn);
    }
}

async function getVideoFrame(videoUrl, timeInSeconds) {
    console.log("===== getVideoFrame: INICIO =====");
    console.log("URL:", videoUrl);
    console.log("Tiempo solicitado:", timeInSeconds);

    return new Promise((resolve, reject) => {
        const video = document.createElement("video");
        video.crossOrigin = "anonymous";
        video.src = videoUrl;
        video.muted = true;

        video.addEventListener("loadeddata", () => {
            console.log("loadeddata → el video cargó metadata");
            console.log("Duración del video:", video.duration);
            console.log("Resolución:", video.videoWidth, "x", video.videoHeight);

            if (timeInSeconds > video.duration) {
                console.error("ERROR: tiempo solicitado mayor a la duración del video");
                resolve(null);
                return;
            }

            console.log("Moviendo video.currentTime =", timeInSeconds);
            video.currentTime = timeInSeconds;
        });

        video.addEventListener("seeked", () => {
            console.log("seeked → posición alcanzada:", video.currentTime);

            const canvas = document.createElement("canvas");
            canvas.width = video.videoWidth;
            canvas.height = video.videoHeight;

            const ctx = canvas.getContext("2d");
            ctx.drawImage(video, 0, 0);

            console.log("Frame dibujado en canvas, convirtiendo a Base64…");

            const base64 = canvas.toDataURL("image/png");

            console.log("Base64 generado, longitud:", base64.length);
            console.log("===== getVideoFrame: FIN =====");

            resolve(base64);
        });

        video.addEventListener("error", (e) => {
            console.error("ERROR en <video>:", e);
            reject(e);
        });

        console.log("Cargando video…");
        video.load();
    });
}

function debugHighlight(id) {
    const elemento = elementosMap.get(id)?.nodo;
    if (!elemento) return;

    // outline amarillo sin afectar el layout
    elemento.style.outline = "2px solid yellow";
    elemento.style.outlineOffset = "0px";
    elemento.style.transition = "outline 0s";

    // cancelar timer previo si existe
    if (elemento._borderTimer) {
        clearTimeout(elemento._borderTimer);
    }

    // nuevo timer para limpiar
    elemento._borderTimer = setTimeout(() => {
        elemento.style.outline = "";
        elemento._borderTimer = null;
    }, 3000);
}

// Diccionario global para manejar múltiples elementos
const debugElements = {};

// -------------------- PINTAR PUNTO --------------------
function pintarPunto(id, posX, posY, ancho = 40, alto = 60, grosor = "1px", color = "red", align = 1, duracionMs = 3000) {
    const toPx = v => (typeof v === "number" ? v + "px" : v);

    let guide = debugElements[id];
    if (!guide) {
        guide = document.createElement("div");
        guide.id = id;
        guide.style.position = "absolute";
        guide.style.pointerEvents = "none";
        guide.style.zIndex = "9999";
        guide.style.left = 0;
        guide.style.top = 0;

        const punto = document.createElement("div");
        punto.className = "dg-punto";
        guide.appendChild(punto);

        const lineaV = document.createElement("div");
        lineaV.className = "dg-lineaV";
        guide.appendChild(lineaV);

        const lineaH = document.createElement("div");
        lineaH.className = "dg-lineaH";
        guide.appendChild(lineaH);

        document.body.appendChild(guide);
        debugElements[id] = guide;
    }

    // actualizar estilos
    const puntoEl = guide.querySelector(".dg-punto");
    const lineaVEl = guide.querySelector(".dg-lineaV");
    const lineaHEl = guide.querySelector(".dg-lineaH");

    if (puntoEl) {
        puntoEl.style.width = "1px";
        puntoEl.style.height = "1px";
        puntoEl.style.background = color;
        puntoEl.style.position = "absolute";
        puntoEl.style.left = "0";
        puntoEl.style.top = "0";
    }
    if (lineaVEl) {
        const altoAbs = Math.abs(alto);

        // Si alto > 0 → hacia abajo
        // Si alto < 0 → hacia arriba
        const topPos = alto < 0 ? alto : 0;

        lineaVEl.style.width = toPx(grosor);
        lineaVEl.style.height = toPx(altoAbs);
        lineaVEl.style.top = toPx(topPos);
        lineaVEl.style.left = "0";
        lineaVEl.style.background = color;
        lineaVEl.style.position = "absolute";
    }

    if (lineaHEl) {
        let leftPos = 0;

        if (align === 0) {
            // borde izquierdo (desde el punto hacia la derecha)
            leftPos = 0;
        } else if (align === 2) {
            // centro
            leftPos = -parseFloat(ancho) / 2;
        } else if (align === 1) {
            // borde derecho (desde el punto hacia la izquierda)
            leftPos = -parseFloat(ancho);
        }

        lineaHEl.style.height = toPx(grosor);
        lineaHEl.style.width = toPx(ancho);
        lineaHEl.style.left = toPx(leftPos);
        lineaHEl.style.top = "0";
        lineaHEl.style.background = color;
        lineaHEl.style.position = "absolute";
    }


    guide.style.display = "";
    guide.style.transform = `translate(${posX}px, ${posY}px)`;

    if (guide._hideTimer) {
        clearTimeout(guide._hideTimer);
        guide._hideTimer = null;
    }

    if (duracionMs > 0) {
        guide._hideTimer = setTimeout(() => {
            guide.style.display = "none";
            guide._hideTimer = null;
        }, duracionMs);
    }
}

// -------------------- PINTAR CUADRO --------------------
function pintarCuadro(id, posX, posY, ancho, alto, grosor = "2px", color = "blue", duracionMs = 3000) {
    let cuadro = debugElements[id];
    console.log(id);
    if (!cuadro) {
        cuadro = document.createElement("div");
        cuadro.id = id;
        cuadro.style.position = "absolute";
        cuadro.style.pointerEvents = "none";
        cuadro.style.zIndex = "9999";



        document.body.appendChild(cuadro);
        debugElements[id] = cuadro;
    }

    cuadro.style.left = posX + "px";
    cuadro.style.top = posY + "px";
    cuadro.style.width = ancho + "px";
    cuadro.style.height = alto + "px";
    cuadro.style.border = `${grosor} solid ${color}`;
    cuadro.style.display = "";
    cuadro.style.boxSizing = "border-box";
    if (cuadro._hideTimer) {
        clearTimeout(cuadro._hideTimer);
        cuadro._hideTimer = null;
    }

    if (duracionMs > 0) {
        cuadro._hideTimer = setTimeout(() => {
            cuadro.style.display = "none";
            cuadro._hideTimer = null;
        }, duracionMs);
    }
}

// -------------------- ELIMINAR ELEMENTO --------------------
function borrarElemento(id) {
    const el = debugElements[id];
    if (el) {
        el.remove();
        delete debugElements[id];
    }
}

function eliminarPorGrupoId(grupoId, retraso = 0, fadeOut = 0) {
    elementosMap.forEach((value, key) => {
        const nodo = value.nodo;
        if (!nodo) return;

        if (nodo.dataset.grupoId === grupoId) {
            const fade =
                fadeOut === -1
                    ? Number(nodo.dataset.fadeOut ?? 0)
                    : fadeOut;

            // Esperar el retraso antes de iniciar la transición
            setTimeout(() => {
                // Asegurar estado inicial
                nodo.style.opacity = getComputedStyle(nodo).opacity;
                nodo.style.transition = `opacity ${fade}ms ease-out`;

                requestAnimationFrame(() => {
                    nodo.style.opacity = "0";
                });

                // Eliminar cuando termina el fade
                setTimeout(() => {
                    if (nodo.tagName === "VIDEO") {
                        nodo.pause();
                        nodo.srcObject = null;
                        nodo.removeAttribute("src"); // Eliminar la referencia al archivo

                    }
                    nodo.remove();
                    elementosMap.delete(key);
                }, fade);

            }, retraso);
        }
    });
}

// -------------------- ELIMINAR TODOS --------------------
function borrarTodos() {
    for (const id in debugElements) {
        debugElements[id].remove();
    }
    Object.keys(debugElements).forEach(k => delete debugElements[k]);
}

