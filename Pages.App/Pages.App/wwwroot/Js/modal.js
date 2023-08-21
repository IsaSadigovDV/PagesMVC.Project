const modal = document.getElementById("cartModal");
const addToCartButton = document.getElementById("addToCartButton");
const closeIcon = document.querySelector(".closeIcon");

console.log(modal);
console.log(addToCartButton);

addToCartButton.addEventListener("click", function () {
  modal.style.visibility = "visible";
  modal.style.opacity = 1;
});

closeIcon.addEventListener("click", function () {
  modal.style.visibility = "hidden";
  modal.style.opacity = 0;
});

// window.addEventListener("click", function (event) {
//   if (event.target === modal) {
//     modal.style.display = "none";
//   }
// });
