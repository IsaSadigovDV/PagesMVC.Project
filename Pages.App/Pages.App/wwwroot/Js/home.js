let menu = document.getElementById("menu");
let links = document.querySelector(".links");
let menuClose = document.getElementById("close-menu");

menu.addEventListener("click", menuToggle);
function menuToggle() {
  links.classList.toggle("active");
}
menuClose.addEventListener("click", closeMenuToogle);
function closeMenuToogle() {
  links.classList.toggle("active");
}