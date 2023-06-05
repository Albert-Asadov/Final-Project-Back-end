$('.responsive').slick({
    dots: false,
    infinite: false,
    speed: 300,
    slidesToShow: 1,
    slidesToScroll: 1,
    nextArrow: '<i class="fa-solid fa-angle-right next_arrow"></i>',
    prevArrow: '<i class="fa-solid fa-chevron-left prev_arrow"></i>',
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 3,
          infinite: true,
          dots: false
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 2
        }
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      }
      // You can unslick at a given breakpoint now by adding:
      // settings: "unslick"
      // instead of a settings object
    ]
  });



  $('.products').slick({
    dots: false,
    infinite: true,
    speed: 300,
    slidesToShow: 4,
    slidesToScroll: 1,
    nextArrow: '<i class="fa-solid fa-angle-right next_arrow"></i>',
    prevArrow: '<i class="fa-solid fa-chevron-left prev_arrow"></i>',
    responsive: [
      {
        breakpoint: 1024,
        settings: {
          slidesToShow: 2,
          slidesToScroll: 2,
          infinite: true,
          dots: false
        }
      },
      {
        breakpoint: 600,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 2
        }
      },
      {
        breakpoint: 480,
        settings: {
          slidesToShow: 1,
          slidesToScroll: 1
        }
      }
      // You can unslick at a given breakpoint now by adding:
      // settings: "unslick"
      // instead of a settings object
    ]
  });

  var WishlistPanel = document.querySelector(".panelWish");
  var wishlistButton = document.querySelector(".wishlist");

wishlistButton.addEventListener("click", function (e) {
    e.stopPropagation();
    WishlistPanel.style.display = "block";
})

window.addEventListener("click", function (e) {
    e.stopPropagation();
    WishlistPanel.style.display = "none";
})


  let input = document.querySelector(".quantityInput")

function handleIncreaseQuantity() {
      input.value++
     
  }
  function handleDEcreaseQuantity(){
    if(input.value <=1){
      
    }
    else{
      input.value--
    }
}

var MainImage = document.querySelector(".mainImage img");

var FalseImages = document.querySelectorAll(".images img")


function MainImageFunction(){
    [...FalseImages].forEach(Image=>{

        Image.onclick = function(){
          [...FalseImages].forEach(falseImg =>{
            falseImg.className = "images"
          })
          let src = Image.getAttribute("src");
          MainImage.src = src;
          Image.className = "images active"
        }

        //Image.onmouseover = function(){
        //  [...FalseImages].forEach(falseImg =>{
        //    falseImg.className = "images"
        //  })
        //  let src = Image.getAttribute("src");
        //  MainImage.src = src;
        //  Image.className = "images active"
        //}


    })

}
MainImageFunction()


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

let up = document.querySelector(".up");
up.onclick = function (e) {
    e.preventDefault();
    window.scrollTo({
        top: 0,
        left: 0,
        behavior: 'smooth'
    });
}




