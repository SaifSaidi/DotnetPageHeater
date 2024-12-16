let heatmapInstance;

function initHeatmap() {
    if (typeof h337 === 'undefined') {
        console.error('Heatmap.js library not loaded. Retrying in 1 second...');
        setTimeout(initHeatmap, 1000);
        return;
    }

    if (!document.getElementById('heatmapContainer')) {
        
        return;
    }

    heatmapInstance = h337.create({
        container: document.getElementById('heatmapContainer'),
        radius: 20,
        maxOpacity: 0.5,
        minOpacity: 0,
        blur: 0.75
    });
}

function drawHeatmap(data) {
    if (!heatmapInstance) {
        console.error('Heatmap not initialized. Initializing now...');
        initHeatmap();
        setTimeout(() => drawHeatmap(data), 1000);
        return;
    }

    const points = [];
    const max = Math.max(...Object.values(data));

    for (const [key, value] of Object.entries(data)) {
        const [x, y] = key.split(',').map(Number);
        points.push({
            x: x,
            y: y - 50,
            value: value
        });
    }

    heatmapInstance.setData({
        max: max,
        data: points
    });
}

function getRelativeCoordinates(element) {
    const rect = element.getBoundingClientRect();
     
    const x = rect.x
    const y = rect.y 
    return { x, y };
}

function getElementPath(element) {
    const path = [];
    while (element && element.nodeType === Node.ELEMENT_NODE) {
        let selector = element.nodeName.toLowerCase();
        if (element.id) {
            selector += `#${element.id}`;
        } else if (element.className) {
            selector += `.${element.className.replace(/\s+/g, ".")}`;
        }
        path.unshift(selector);
        element = element.parentNode;
    }
    return path.join(" > ");
}


function recordHeatmap(clickedElement) { 
    const { x, y } = getRelativeCoordinates(clickedElement);
    const elementPath = getElementPath(clickedElement);
    const json = JSON.stringify({
        url: window.location.pathname,
        x: parseFloat(x.toFixed(2)),
        y: parseFloat(y.toFixed(2)),
        elementPath,
        viewportWidth: window.innerWidth,
        viewportHeight: window.innerHeight
    });

    fetch('/api/heatmap/record', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: json,
    });

    console.log(json)
}

function recordEvent(eventName, element) {

    const elementPath = element ? getElementPath(element) : null;
    const json = JSON.stringify({
        eventName,
        url: window.location.pathname,
        elementPath,
        viewportWidth: window.innerWidth,
        viewportHeight: window.innerHeight
    });
    fetch('/api/heatmap/event', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: json,
    });
    console.log(json)
}

// example: use static side js
document.getElementById('testBtn')
    .addEventListener('click', function (e) {
        alert("Collect!") 
        recordEvent("click_testBtn_event", e.target);
    });

// Initialize heatmap when the script loads 
window.addEventListener("DOMContentLoaded", initHeatmap);

