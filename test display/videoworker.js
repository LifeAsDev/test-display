const videos = [];

self.onmessage = (e) => {
    if (e.data.type === "init") {
        const { canvas, videoData } = e.data;
        self.canvas = canvas;
        self.ctx = canvas.getContext("2d");
        videos.length = 0;

        // Reconstruct video objects
        videoData.forEach(v => videos.push(v));
        draw();
    } else if (e.data.type === "updateVideo") {
        // Para agregar nuevos videos en tiempo real
        videos.push(e.data.video);
    }
};

function draw() {
    if (!self.ctx) return;
    self.ctx.clearRect(0, 0, self.canvas.width, self.canvas.height);

    videos.forEach(v => {
        const vid = v.video;
        if (!vid.paused && !vid.ended) {
            self.ctx.globalAlpha = v.opacity ?? 1;
            self.ctx.drawImage(vid, v.x, v.y, v.width, v.height);
        }
    });

    requestAnimationFrame(draw);
}

