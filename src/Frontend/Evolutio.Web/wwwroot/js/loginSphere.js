// Three.js sphere of interactive particles for login branding panel.
// Loaded via dynamic import only on the login page.

import * as THREE from "https://unpkg.com/three@0.160.0/build/three.module.js";

/** @type {Map<string, any>} */
const instances = new Map();

function createParticlesOnSphere(count, radius) {
  // Fibonacci sphere distribution
  const positions = new Float32Array(count * 3);
  const basePositions = new Float32Array(count * 3);

  const offset = 2 / count;
  const increment = Math.PI * (3 - Math.sqrt(5));

  for (let i = 0; i < count; i++) {
    const y = ((i * offset) - 1) + (offset / 2);
    const r = Math.sqrt(1 - y * y);
    const phi = i * increment;
    const x = Math.cos(phi) * r;
    const z = Math.sin(phi) * r;

    const px = x * radius;
    const py = y * radius;
    const pz = z * radius;

    const idx = i * 3;
    positions[idx] = px;
    positions[idx + 1] = py;
    positions[idx + 2] = pz;

    basePositions[idx] = px;
    basePositions[idx + 1] = py;
    basePositions[idx + 2] = pz;
  }

  return { positions, basePositions };
}

export function initLoginSphere(containerId) {
  const container = document.getElementById(containerId);
  if (!container) return;

  // Avoid double-init
  if (instances.has(containerId)) return;

  const scene = new THREE.Scene();

  const camera = new THREE.PerspectiveCamera(40, 1, 0.1, 1000);
  camera.position.set(0, 0, 4.2);

  const renderer = new THREE.WebGLRenderer({
    antialias: true,
    alpha: true,
    powerPreference: "high-performance",
  });
  renderer.setPixelRatio(Math.min(window.devicePixelRatio || 1, 2));
  renderer.setClearColor(0x000000, 0);
  container.appendChild(renderer.domElement);

  const group = new THREE.Group();
  scene.add(group);

  const particleCount = 2800;
  const sphereRadius = 1.35;
  const { positions, basePositions } = createParticlesOnSphere(particleCount, sphereRadius);

  const geometry = new THREE.BufferGeometry();
  geometry.setAttribute("position", new THREE.BufferAttribute(positions, 3));

  const material = new THREE.PointsMaterial({
    color: 0x76ff03,
    size: 0.018,
    sizeAttenuation: true,
    transparent: true,
    opacity: 0.9,
    depthWrite: false,
    blending: THREE.AdditiveBlending,
  });

  const points = new THREE.Points(geometry, material);
  group.add(points);

  // Subtle glow core
  const coreGeom = new THREE.SphereGeometry(0.75, 32, 32);
  const coreMat = new THREE.MeshBasicMaterial({
    color: 0x76ff03,
    transparent: true,
    opacity: 0.05,
  });
  const core = new THREE.Mesh(coreGeom, coreMat);
  group.add(core);

  // Ponteiro: posição global para distância; NDC quando dentro da zona de 60px
  const ACTIVATION_DISTANCE_PX = 60;
  let lastPointer = null; // { x, y } em client coordinates

  const _rayOrigin = new THREE.Vector3();
  const _rayDirection = new THREE.Vector3();
  const _targetOnSphere = new THREE.Vector3();
  const _invGroupMatrix = new THREE.Matrix4();

  function distanceToRect(px, py, rect) {
    const cx = Math.max(rect.left, Math.min(rect.right, px));
    const cy = Math.max(rect.top, Math.min(rect.bottom, py));
    return Math.hypot(px - cx, py - cy);
  }

  function raySphereIntersect(origin, direction, sphereRadius) {
    const a = direction.dot(direction);
    const b = 2 * origin.dot(direction);
    const c = origin.dot(origin) - sphereRadius * sphereRadius;
    const disc = b * b - 4 * a * c;
    if (disc < 0) return null;
    const t = (-b - Math.sqrt(disc)) / (2 * a);
    if (t <= 0) return null;
    const p = origin.clone().add(direction.clone().multiplyScalar(t));
    return p;
  }

  function onPointerMove(ev) {
    lastPointer = { x: ev.clientX, y: ev.clientY };
  }

  document.addEventListener("pointermove", onPointerMove, { passive: true });

  // Resize
  function resize() {
    const w = Math.max(1, container.clientWidth);
    const h = Math.max(1, container.clientHeight);
    renderer.setSize(w, h, false);
    camera.aspect = w / h;
    camera.updateProjectionMatrix();
  }

  const ro = new ResizeObserver(resize);
  ro.observe(container);
  resize();

  let raf = 0;
  let t0 = performance.now();

  function animate(now) {
    raf = requestAnimationFrame(animate);
    const dt = Math.min(0.05, (now - t0) / 1000);
    t0 = now;
    const time = now * 0.001;

    // Rotação na metade da velocidade anterior
    group.rotation.x = Math.sin(time * 0.175) * 0.08;
    group.rotation.y = time * 0.11;

    // Pulsação em ritmo de coração (2 batidas leves + pausa)
    const heartPeriod = 1.05;
    const heartPhase = (time % heartPeriod) / heartPeriod;
    const b1 = Math.exp(-Math.pow((heartPhase - 0.07) / 0.06, 2));
    const b2 = Math.exp(-Math.pow((heartPhase - 0.20) / 0.06, 2));
    const pulse = (b1 * 0.6 + b2 * 0.4) * 0.014;
    const scale = 1 + pulse;
    group.scale.setScalar(scale);

    // Zona de atuação: reação a partir de 60px da esfera, com intensidade pela distância
    let pointerNDC = null;
    let proximityFactor = 0;
    if (lastPointer) {
      const rect = container.getBoundingClientRect();
      const distPx = distanceToRect(lastPointer.x, lastPointer.y, rect);
      if (distPx <= ACTIVATION_DISTANCE_PX) {
        proximityFactor = Math.max(0.12, 1 - distPx / ACTIVATION_DISTANCE_PX);
        const nearestX = Math.max(rect.left, Math.min(rect.right, lastPointer.x));
        const nearestY = Math.max(rect.top, Math.min(rect.bottom, lastPointer.y));
        const localX = (nearestX - rect.left) / rect.width;
        const localY = (nearestY - rect.top) / rect.height;
        pointerNDC = { x: localX * 2 - 1, y: -(localY * 2 - 1) };
      }
    }

    // Ponto de atração na esfera (onde o ponteiro aponta), em espaço local do group
    let targetLocal = null;
    if (pointerNDC && proximityFactor > 0) {
      camera.getWorldPosition(_rayOrigin);
      _rayDirection.set(pointerNDC.x, pointerNDC.y, 0.5).unproject(camera).sub(_rayOrigin).normalize();
      const hit = raySphereIntersect(_rayOrigin, _rayDirection, sphereRadius);
      if (hit) {
        group.updateMatrixWorld(true);
        _invGroupMatrix.copy(group.matrixWorld).invert();
        _targetOnSphere.copy(hit).applyMatrix4(_invGroupMatrix);
        targetLocal = _targetOnSphere;
      }
    }

    const posAttr = geometry.getAttribute("position");
    const attractionStrength = 0.28 * proximityFactor;
    const attractionRadius = 0.85;
    const waveFreq = 2.2;
    const waveSpeed = 3.5;
    const wobble = Math.sin(time * 1.4) * 0.025;

    for (let i = 0; i < particleCount; i++) {
      const idx = i * 3;
      const bx = basePositions[idx];
      const by = basePositions[idx + 1];
      const bz = basePositions[idx + 2];

      let dx = 0, dy = 0, dz = 0;

      if (targetLocal && proximityFactor > 0) {
        const dist = Math.hypot(targetLocal.x - bx, targetLocal.y - by, targetLocal.z - bz);
        const falloff = Math.exp(-(dist * dist) / (2 * attractionRadius * attractionRadius));
        const wave = 0.55 + 0.45 * Math.sin(time * waveSpeed + dist * waveFreq);
        const k = attractionStrength * falloff * wave;
        dx = (targetLocal.x - bx) * k;
        dy = (targetLocal.y - by) * k;
        dz = (targetLocal.z - bz) * k;
      }

      const phase = (i % 97) * 0.17;
      const subtle = Math.sin(time * 1.1 + phase) * wobble;
      const invLen = 1 / Math.max(1e-6, Math.hypot(bx, by, bz));
      dx += bx * invLen * subtle;
      dy += by * invLen * subtle;
      dz += bz * invLen * subtle;

      posAttr.array[idx] = bx + dx;
      posAttr.array[idx + 1] = by + dy;
      posAttr.array[idx + 2] = bz + dz;
    }
    posAttr.needsUpdate = true;

    renderer.render(scene, camera);
  }

  raf = requestAnimationFrame(animate);

  instances.set(containerId, {
    container,
    renderer,
    scene,
    camera,
    geometry,
    material,
    group,
    points,
    core,
    ro,
    raf,
    onPointerMove,
  });
}

export function disposeLoginSphere(containerId) {
  const inst = instances.get(containerId);
  if (!inst) return;

  cancelAnimationFrame(inst.raf);
  try {
    inst.ro.disconnect();
  } catch {}
  try {
    document.removeEventListener("pointermove", inst.onPointerMove);
  } catch {}

  try {
    inst.geometry.dispose();
    inst.material.dispose();
    inst.renderer.dispose();
  } catch {}

  try {
    if (inst.renderer?.domElement?.parentElement) {
      inst.renderer.domElement.parentElement.removeChild(inst.renderer.domElement);
    }
  } catch {}

  instances.delete(containerId);
}

