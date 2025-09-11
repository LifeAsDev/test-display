
                const canvas = document.getElementById("canvas");
                const gl = canvas.getContext("webgl", {premultipliedAlpha: false });
                canvas.width = window.innerWidth;
                canvas.height = window.innerHeight;

                const vsSource = `
                attribute vec2 a_position;
                attribute vec2 a_texCoord;
                uniform vec4 u_rect;
                varying vec2 v_texCoord;
                void main() {
                    vec2 pos = a_position * u_rect.zw + u_rect.xy;
                gl_Position = vec4(pos, 0.0, 1.0);
                v_texCoord = a_texCoord;
}
                `;

                const fsSource = `
                precision mediump float;
                varying vec2 v_texCoord;
                uniform sampler2D u_texture;
                uniform float u_alpha;
                void main() {
                    vec4 color = texture2D(u_texture, v_texCoord);
                gl_FragColor = vec4(color.rgb, color.a * u_alpha);
}
                `;

                function compileShader(src, type) {
    const shader = gl.createShader(type);
                gl.shaderSource(shader, src);
                gl.compileShader(shader);
                if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) console.error(gl.getShaderInfoLog(shader));
                return shader;
}

                const vs = compileShader(vsSource, gl.VERTEX_SHADER);
                const fs = compileShader(fsSource, gl.FRAGMENT_SHADER);
                const program = gl.createProgram();
                gl.attachShader(program, vs);
                gl.attachShader(program, fs);
                gl.linkProgram(program);
                gl.useProgram(program);

                const positionBuffer = gl.createBuffer();
                gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer);
                gl.bufferData(gl.ARRAY_BUFFER, new Float32Array([
                -1, -1, 1, -1, -1, 1,
                1, -1, 1, 1, -1, 1
                ]), gl.STATIC_DRAW);

                const texCoordBuffer = gl.createBuffer();
                gl.bindBuffer(gl.ARRAY_BUFFER, texCoordBuffer);
                gl.bufferData(gl.ARRAY_BUFFER, new Float32Array([
                0, 0, 1, 0, 0, 1,
                1, 0, 1, 1, 0, 1
                ]), gl.STATIC_DRAW);

                const a_position = gl.getAttribLocation(program, "a_position");
                gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer);
                gl.enableVertexAttribArray(a_position);
                gl.vertexAttribPointer(a_position, 2, gl.FLOAT, false, 0, 0);

                const a_texCoord = gl.getAttribLocation(program, "a_texCoord");
                gl.bindBuffer(gl.ARRAY_BUFFER, texCoordBuffer);
                gl.enableVertexAttribArray(a_texCoord);
                gl.vertexAttribPointer(a_texCoord, 2, gl.FLOAT, false, 0, 0);

                const u_texture = gl.getUniformLocation(program, "u_texture");
                const u_alpha = gl.getUniformLocation(program, "u_alpha");
                const u_rect = gl.getUniformLocation(program, "u_rect");
                gl.uniform1i(u_texture, 0);

                const elementosMap = new Map();

                async function loadVideo(Id, grupo, url, x, y, w, h, alpha = 1.0) {
    const video = document.createElement("video");
                video.src = url;
                video.loop = true;
                video.muted = true;
                video.playsInline = true;
                await video.play();

                if (w === 0) w = video.videoWidth;
                if (h === 0) h = video.videoHeight;

                const tex = gl.createTexture();
                gl.bindTexture(gl.TEXTURE_2D, tex);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);

                gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, video.videoWidth, video.videoHeight, 0, gl.RGBA, gl.UNSIGNED_BYTE, null);

                elementosMap.set(Id, {tipo: "video", grupo, video, tex, x, y, w, h, alpha });

                function update() {
        if (!video.paused && !video.ended) {
                    gl.bindTexture(gl.TEXTURE_2D, tex);
                gl.texSubImage2D(gl.TEXTURE_2D, 0, 0, 0, gl.RGBA, gl.UNSIGNED_BYTE, video);
        }
                requestAnimationFrame(update);
    }
                update();
}

                async function loadImage(Id, grupo, url, x, y, w, h, alpha = 1.0) {
    const img = new Image();
                img.src = url;
                await img.decode();

                if (w === 0) w = img.width;
                if (h === 0) h = img.height;

                const tex = gl.createTexture();
                gl.bindTexture(gl.TEXTURE_2D, tex);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);

                gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, gl.RGBA, gl.UNSIGNED_BYTE, img);

                elementosMap.set(Id, {tipo: "image", grupo, tex, x, y, w, h, alpha });
}

                function loadText(Id, grupo, textoCfg, x, y, w, h, alpha = 1.0) {
    const canvasText = document.createElement("canvas");
                const ctx = canvasText.getContext("2d");
                ctx.font = `${textoCfg.FontSize || 24}px ${textoCfg.FontFamily || "sans-serif"}`;
                const metrics = ctx.measureText(textoCfg.Contenido || "");
                canvasText.width = metrics.width;
                canvasText.height = textoCfg.FontSize * 1.2;
                ctx.font = `${textoCfg.FontSize || 24}px ${textoCfg.FontFamily || "sans-serif"}`;
                ctx.fillStyle = textoCfg.Color || "white";
                ctx.fillText(textoCfg.Contenido || "", 0, textoCfg.FontSize);

                if (w === 0) w = canvasText.width;
                if (h === 0) h = canvasText.height;

                const tex = gl.createTexture();
                gl.bindTexture(gl.TEXTURE_2D, tex);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_S, gl.CLAMP_TO_EDGE);
                gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_WRAP_T, gl.CLAMP_TO_EDGE);

                gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA, gl.RGBA, gl.UNSIGNED_BYTE, canvasText);

                elementosMap.set(Id, {tipo: "text", grupo, tex, x, y, w, h, alpha });
}

                function agregarObjetoDisplay(config) {
    const {IdGrupo, Id, Url = "", Texto = null, Ancho = 0, Alto = 0,
                    PosX = 0, PosY = 0, Opacidad = 100} = config;

                if (Url) {
        const ext = Url.split(".").pop().toLowerCase();
                if (["mp4", "webm", "ogg", "avi"].includes(ext)) {
                    loadVideo(Id, IdGrupo, Url, PosX, PosY, Ancho, Alto, Opacidad / 100);
        } else {
                    loadImage(Id, IdGrupo, Url, PosX, PosY, Ancho, Alto, Opacidad / 100);
        }
    } else if (Texto) {
                    loadText(Id, IdGrupo, Texto, PosX, PosY, Ancho, Alto, Opacidad / 100);
    }
}

                function render() {
                    gl.viewport(0, 0, canvas.width, canvas.height);
                gl.clearColor(0, 0, 0, 0);
                gl.clear(gl.COLOR_BUFFER_BIT);

                elementosMap.forEach(({tex, alpha, x, y, w, h}) => {
                    gl.bindTexture(gl.TEXTURE_2D, tex);
                gl.uniform1f(u_alpha, alpha);

                const clipX = (x / canvas.width) * 2 - 1;
                const clipY = 1 - (y / canvas.height) * 2;
                const clipW = (w / canvas.width) * 2;
                const clipH = (h / canvas.height) * 2;

                gl.uniform4f(u_rect, clipX, clipY, clipW, -clipH);
                gl.drawArrays(gl.TRIANGLES, 0, 6);
    });

                requestAnimationFrame(render);
}
                render();