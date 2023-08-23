const modal = document.getElementById("cartModal");
const addToCartButton = document.getElementById("addToCartButton");
const closeIcon = document.querySelector(".closeIcon");

closeIcon.addEventListener("click", closeModal);
modal.addEventListener("click", closeModal);

addToCartButton.addEventListener("click", function () {
  modal.style.visibility = "visible";
  modal.style.opacity = 1;
});

function closeModal() {
  modal.style.visibility = "hidden";
  modal.style.opacity = 0;
}
