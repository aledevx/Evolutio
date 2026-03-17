// Liquid click ripple effect for main layout background
// Triggered on pointer down inside a container.

const instances = new Map();

export function initLiquidClick(containerId) {
  const el = document.getElementById(containerId);
  if (!el || instances.has(containerId)) return;

  const handler = (ev) => {
    const rect = el.getBoundingClientRect();
    const x = ev.clientX - rect.left;
    const y = ev.clientY - rect.top;

    const ripple = document.createElement("span");
    ripple.className = "liquid-ripple";
    ripple.style.left = `${x}px`;
    ripple.style.top = `${y}px`;

    el.appendChild(ripple);

    ripple.addEventListener(
      "animationend",
      () => {
        ripple.remove();
      },
      { once: true }
    );
  };

  el.addEventListener("pointerdown", handler, { passive: true });
  instances.set(containerId, { el, handler });
}

export function disposeLiquidClick(containerId) {
  const inst = instances.get(containerId);
  if (!inst) return;

  inst.el.removeEventListener("pointerdown", inst.handler);
  instances.delete(containerId);
}

