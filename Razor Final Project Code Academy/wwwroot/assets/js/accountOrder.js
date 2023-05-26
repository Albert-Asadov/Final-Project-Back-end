var login = document.querySelector(".login");
var panel = document.querySelector(".panel");
var empty = document.querySelector(".empty")

var iconAcc = document.querySelector(".accountIcon");
var panelAccount = document.querySelector(".panelMyAcc");
var emptyAccount = document.querySelector(".emptyMyAcc")

var iconBasket = document.querySelector(".basketIcon");
var panelAccountBasket = document.querySelector(".panelBasketItems");
var emptyAccountBasket = document.querySelector(".emptyBasketItems")

var hambMenu  =document.querySelector(".menu");
var hambMenuCateg = document.querySelector(".hamburgerMenu");


hambMenu.addEventListener("click", function(e){
  e.stopPropagation()

  hambMenuCateg.setAttribute("style","display:block !important")

})

login.addEventListener("click", function(e){
   e.stopPropagation()
    empty.setAttribute("style","display:block !important")
    panel.setAttribute("style","display:block !important")

})

iconAcc.addEventListener("click", function(e){
  e.stopPropagation();
  emptyAccount.setAttribute("style","display:block !important")
  panelAccount.setAttribute("style","display:block !important")

})

iconBasket.addEventListener("click", function(e){
  
  e.stopPropagation();
  panelAccountBasket.setAttribute("style","display:block !important")
  emptyAccountBasket.setAttribute("style","display:block !important")
  
})

window.addEventListener("click",function(){
  emptyAccount.setAttribute("style","display:none !important")
  panelAccount.setAttribute("style","display:none !important")

     empty.setAttribute("style","display:none !important")
    panel.setAttribute("style","display:none !important")

    panelAccountBasket.setAttribute("style","display:none !important")
    emptyAccountBasket.setAttribute("style","display:none !important")

    hambMenuCateg.setAttribute("style","display:none !important")
})



const mainHeader = document.getElementById('MainHeader');

window.addEventListener("scroll", function () {
  if (window.scrollY >= 100 || window.pageYOffset >= 100) {
    mainHeader.classList.add('headerScroll');
  } else {
    mainHeader.classList.remove('headerScroll')
  }
});

let i = document.querySelector(".up")
window.addEventListener("scroll", function () {
  if (window.scrollY >= 250 || window.pageYOffset >= 250) {
    i.classList.add('i');
    i.style.opacity = "1";
  } else {
    i.classList.remove('i');
    i.style.opacity = "0";
  }
});