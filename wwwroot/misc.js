window.isBottomPart = (y) => {
    console.log(y, window.innerHeight / 2);
    return y > window.innerHeight / 2;
};

window.fitsToTheRight = (x, size) => {
    return x + size < window.innerWidth;
}