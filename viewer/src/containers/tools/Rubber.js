import { v4 } from 'uuid';

export const TOOL_RUBBER = 'rubber';

export default (context) => {
  let stroke = null;
  let points = [];

  const onMouseDown = (x, y, color, size) => {
    stroke = {
      id: v4(),
      tool: TOOL_RUBBER,
      color,
      size,
      points: [{ x, y }]
    };
    return [stroke];
  };

  const drawLine = (item, start, { x, y }) => {
    context.save();
    context.lineJoin = 'round';
    context.lineCap = 'square';
    context.beginPath();
    context.lineWidth = item.size;
    context.strokeStyle = ("rgba(255,255,255,255)");
    context.globalCompositeOperation = "destination-out"; 
    context.moveTo(start.x, start.y);
    context.lineTo(x, y);
    context.closePath();
    context.stroke();
    context.restore();
  };

  const onMouseMove = (x, y) => {
    if (!stroke) return [];
    const newPoint = { x, y };
    const start = stroke.points.slice(-1)[0];
    drawLine(stroke, start, newPoint);
    stroke.points.push(newPoint);
    points.push(newPoint);

    return [stroke];
  };

  const onDebouncedMouseMove = () => {
    const debouncedPoints = points;
    points = [];
    return [stroke, debouncedPoints];
  };

  const onMouseUp = (x, y) => {
    if (!stroke) return;
    onMouseMove(x, y);
    points = [];
    const item = stroke;
    stroke = null;
    return [item];
  };

  const draw = (item, animate) => {
    let time = 0;
    let i = 0;
    const j = item.points.length;
    for (i, j; i < j; i++) {
      if (!item.points[i - 1]) continue;
      if (animate) {
        setTimeout(drawLine.bind(null, item, item.points[i - 1], item.points[i]), time);
        time += 10;
      } else {
        drawLine(item, item.points[i - 1], item.points[i]);
      }
    }
  };

  return {
    onMouseDown,
    onMouseMove,
    onDebouncedMouseMove,
    onMouseUp,
    draw,
  };
};