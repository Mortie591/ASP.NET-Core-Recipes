/**
* Template Name: Yummy
* Updated: Mar 10 2023 with Bootstrap v5.2.3
* Template URL: https://bootstrapmade.com/yummy-bootstrap-restaurant-website-template/
* Author: BootstrapMade.com
* License: https://bootstrapmade.com/license/
*/
document.addEventListener('DOMContentLoaded', () => {
  "use strict";

  /**
   * Preloader
   */
  const preloader = document.querySelector('#preloader');
  if (preloader) {
    window.addEventListener('load', () => {
      preloader.remove();
    });
  }

  /**
   * Sticky header on scroll
   */
  const selectHeader = document.querySelector('#header');
  if (selectHeader) {
    document.addEventListener('scroll', () => {
      window.scrollY > 100 ? selectHeader.classList.add('sticked') : selectHeader.classList.remove('sticked');
    });
  }
    /**
   * Custom
   */
  //Add Ingredients row
    let ingredientsParentElement = document.querySelector("ul[name='ingredients']");
    let sectionParentElement = document.querySelector("ul[name='section-ul']");

    let ingredientsFieldset = document.querySelector('#ingredients-fieldset');
    let ingredientRow = document.querySelector('#ingredients-row');
    let addIngredientButtonElement = document.querySelector('#add-ingredient'); 
    let removeIngredientButtonElement = document.querySelector('#remove-ingredient'); 
    

    const AddIngredientRow = function (parentNode) {
        
        let childNode = ingredientRow.cloneNode(true);
        parentNode.insertBefore(childNode, event.target);
        console.log("Added new ingredient line")
    };

    addIngredientButtonElement.addEventListener('click', (event) => {
        event.preventDefault();
        let parent = event.target.parentNode;
        AddIngredientRow(parent);
    });


    //Remove Ingredients row
    const RemoveIngredientRow = function (parentNode) {
        let liElements = parentNode.querySelectorAll('li');
        if (liElements.length > 0) {
            let lastListElement = liElements[liElements.length - 1];
            parentNode.removeChild(lastListElement);
            console.log('Removed last ingredient row ${lastListElement}');
        } else {
            console.log('No elements to remove')
        }
        
    };
    removeIngredientButtonElement.addEventListener('click', (event) => {
        event.preventDefault();
        let parent = event.target.parentNode;
        RemoveIngredientRow(parent);
    });


    //Add section
    /**
         * <div class="col-lg-4 col-md-6">
               <label for="section-name"><strong>Section Name</strong></label>
               <input type="text" name="section-name" class="form-control" id="section-name" placeholder="Section Name">
           </div>
         */
    document.querySelector('#add-section').addEventListener('click', function (event) {
        event.preventDefault();

        let sectionElement = document.createElement('ul');
        sectionElement.setAttribute('class', 'form-group mt-3 mb-3');
        sectionElement.setAttribute('id', 'section-ul');

        let labelElement = document.createElement('label');
        labelElement.setAttribute('for', 'section-name');

        let strongElement = document.createElement('strong');
        strongElement.innerText = 'Section Name';

        labelElement.appendChild(strongElement);

        let inputElement = document.createElement('input');
        inputElement.setAttribute('type', 'text');
        inputElement.setAttribute('name', 'section-name');
        inputElement.setAttribute('class', 'form-control col-lg-4 col-md-6');
        inputElement.setAttribute('id', 'section-name');
        inputElement.setAttribute('placeholder', 'Section Name');

        let ingredientRow = ingredientsParentElement.querySelector('#ingredients-row').cloneNode(true);

        var addButtonElementSection = document.querySelector('#add-ingredient').cloneNode(true);
        var removeButtonElementSection = document.querySelector('#remove-ingredient').cloneNode(true);

        addButtonElementSection.addEventListener('click', (event) => {
            event.preventDefault();
            let parent = event.target.parentNode;
            AddIngredientRow(parent);
        });

        removeButtonElementSection.addEventListener('click', (event) => {
            event.preventDefault();
            let parent = event.target.parentNode;
            RemoveIngredientRow(parent);
        });

        sectionElement.appendChild(labelElement);
        sectionElement.appendChild(inputElement);
        sectionElement.appendChild(ingredientRow);
        sectionElement.appendChild(addButtonElementSection);
        sectionElement.appendChild(removeButtonElementSection);

        ingredientsFieldset.appendChild(sectionElement);
        console.log("Added new section")
    });

    //Remove Section
    const RemoveSection = function (parentNode) {
        let sectionElements = parentNode.querySelectorAll("ul[id='section-ul']");
        if (sectionElements.length > 0) {
            let lastSectionElement = sectionElements[sectionElements.length - 1];
            parentNode.removeChild(lastSectionElement);
            console.log('Removed last ingredient row ${lastSectionElement}');
        } else {
            console.log('No elements to remove')
        }

    };
    document.querySelector('#remove-section').addEventListener('click', (event) =>
    {
        event.preventDefault();
            let parent = event.target.parentNode.parentNode;
            RemoveSection(parent);
    });


    //Add Nutrient row
    let nutrientsParentElement = document.querySelector("ul[name='nutrients']");

    let nutrientRow = document.querySelector('#nutrients-row');
    let addNutrientButtonElement = document.querySelector('#add-nutrient');
    let removeNutrientButtonElement = document.querySelector('#remove-nutrient');


    const AddNutrientRow = function (parentNode) {
        let childNode = nutrientRow.cloneNode(true);
        parentNode.insertBefore(childNode, event.target);
        console.log("Added new nutrient row")
    };

    addNutrientButtonElement.addEventListener('click', (event) => {
        event.preventDefault();
        let parent = event.target.parentNode;
        AddNutrientRow(parent);
    });


    //Remove Nutrients row
    const RemoveNutrientRow = function (parentNode) {
        let liElements = parentNode.querySelectorAll('li');
        if (liElements.length > 0) {
            let lastListElement = liElements[liElements.length - 1];
            parentNode.removeChild(lastListElement);
            console.log('Removed last nutrient row');
        } else {
            console.log('No elements to remove')
        }

    };
    removeNutrientButtonElement.addEventListener('click', (event) => {
        event.preventDefault();
        let parent = event.target.parentNode;
        RemoveNutrientRow(parent);
    });


    /**
   * Navbar links active state on scroll
   */
  let navbarlinks = document.querySelectorAll('#navbar a');

  function navbarlinksActive() {
    navbarlinks.forEach(navbarlink => {

      if (!navbarlink.hash) return;

      let section = document.querySelector(navbarlink.hash);
      if (!section) return;

      let position = window.scrollY + 200;

      if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
        navbarlink.classList.add('active');
      } else {
        navbarlink.classList.remove('active');
      }
    })
  }
  window.addEventListener('load', navbarlinksActive);
  document.addEventListener('scroll', navbarlinksActive);

  /**
   * Mobile nav toggle
   */
  const mobileNavShow = document.querySelector('.mobile-nav-show');
  const mobileNavHide = document.querySelector('.mobile-nav-hide');

  document.querySelectorAll('.mobile-nav-toggle').forEach(el => {
    el.addEventListener('click', function(event) {
      event.preventDefault();
      mobileNavToogle();
    })
  });

  function mobileNavToogle() {
    document.querySelector('body').classList.toggle('mobile-nav-active');
    mobileNavShow.classList.toggle('d-none');
    mobileNavHide.classList.toggle('d-none');
  }

  /**
   * Hide mobile nav on same-page/hash links
   */
  document.querySelectorAll('#navbar a').forEach(navbarlink => {

    if (!navbarlink.hash) return;

    let section = document.querySelector(navbarlink.hash);
    if (!section) return;

    navbarlink.addEventListener('click', () => {
      if (document.querySelector('.mobile-nav-active')) {
        mobileNavToogle();
      }
    });

  });

  /**
   * Toggle mobile nav dropdowns
   */
  const navDropdowns = document.querySelectorAll('.navbar .dropdown > a');

  navDropdowns.forEach(el => {
    el.addEventListener('click', function(event) {
      if (document.querySelector('.mobile-nav-active')) {
        event.preventDefault();
        this.classList.toggle('active');
        this.nextElementSibling.classList.toggle('dropdown-active');

        let dropDownIndicator = this.querySelector('.dropdown-indicator');
        dropDownIndicator.classList.toggle('bi-chevron-up');
        dropDownIndicator.classList.toggle('bi-chevron-down');
      }
    })
  });

  /**
   * Scroll top button
   */
  const scrollTop = document.querySelector('.scroll-top');
  if (scrollTop) {
    const togglescrollTop = function() {
      window.scrollY > 100 ? scrollTop.classList.add('active') : scrollTop.classList.remove('active');
    }
    window.addEventListener('load', togglescrollTop);
    document.addEventListener('scroll', togglescrollTop);
    scrollTop.addEventListener('click', window.scrollTo({
      top: 0,
      behavior: 'smooth'
    }));
  }

  /**
   * Initiate glightbox
   */
  const glightbox = GLightbox({
    selector: '.glightbox'
  });

  /**
   * Initiate pURE cOUNTER
   */
  new PureCounter();

  /**
   * Init swiper slider with 1 slide at once in desktop view
   */
  new Swiper('.slides-1', {
    speed: 600,
    loop: true,
    autoplay: {
      delay: 5000,
      disableOnInteraction: false
    },
    slidesPerView: 'auto',
    pagination: {
      el: '.swiper-pagination',
      type: 'bullets',
      clickable: true
    },
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev',
    }
  });

  /**
   * Init swiper slider with 3 slides at once in desktop view
   */
  new Swiper('.slides-3', {
    speed: 600,
    loop: true,
    autoplay: {
      delay: 5000,
      disableOnInteraction: false
    },
    slidesPerView: 'auto',
    pagination: {
      el: '.swiper-pagination',
      type: 'bullets',
      clickable: true
    },
    navigation: {
      nextEl: '.swiper-button-next',
      prevEl: '.swiper-button-prev',
    },
    breakpoints: {
      320: {
        slidesPerView: 1,
        spaceBetween: 40
      },

      1200: {
        slidesPerView: 3,
      }
    }
  });

  /**
   * Gallery Slider
   */
  new Swiper('.gallery-slider', {
    speed: 400,
    loop: true,
    centeredSlides: true,
    autoplay: {
      delay: 5000,
      disableOnInteraction: false
    },
    slidesPerView: 'auto',
    pagination: {
      el: '.swiper-pagination',
      type: 'bullets',
      clickable: true
    },
    breakpoints: {
      320: {
        slidesPerView: 1,
        spaceBetween: 20
      },
      640: {
        slidesPerView: 3,
        spaceBetween: 20
      },
      992: {
        slidesPerView: 5,
        spaceBetween: 20
      }
    }
  });

  /**
   * Animation on scroll function and init
   */
  function aos_init() {
    AOS.init({
      duration: 1000,
      easing: 'ease-in-out',
      once: true,
      mirror: false
    });
  }
  window.addEventListener('load', () => {
    aos_init();
  });

});