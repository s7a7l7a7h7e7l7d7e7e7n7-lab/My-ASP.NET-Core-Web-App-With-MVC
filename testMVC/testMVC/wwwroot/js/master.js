// =========================================================================
// 1. استرجاع وتطبيق خيار اللون المحفوظ مسبقاً (Local Storage Color Option)
// =========================================================================
// عند تحميل الصفحة، نفحص التخزين المحلي للمتصفح (localStorage) لمعرفة ما إذا كان المستخدم قد اختار لوناً سابقاً
let mainColor = localStorage.getItem("color_option");

if (mainColor !== null) {
    // تعيين اللون المحفوظ على جذر الصفحة (:root) ليتغير في كامل عناصر الـ CSS فوراً
    document.documentElement.style.setProperty('--main--color', mainColor);

    // إزالة فئة النشاط (active) من كافة الدوائر اللونية ثم وضعها على الدائرة المطابقة للون المحفوظ
    document.querySelectorAll(".color-list li").forEach(el => {
        el.classList.remove("active");
        if (el.dataset.color === mainColor) {
            el.classList.add("active");
        }
    });

    // تحديث خلفية أيقونة الترس (.im) والحاوية (.toggle) لتطابق اللون المحفوظ
    const initialIm = document.querySelector(".setting-box .toggle .im");
    if (initialIm) {
        initialIm.style.setProperty("background-color", mainColor);
    }
    const initialToggle = document.querySelector(".setting-box .toggle");
    if (initialToggle) {
        initialToggle.style.setProperty("background-color", mainColor);
    }
}

// =========================================================================
// 2. التحكم في فتح وإغلاق صندوق الإعدادات ودوران أيقونة الترس (Toggle Settings)
// =========================================================================
document.querySelector(".toggle .fa-gear").onclick = function () {
    // تشغيل أو إيقاف تأثير الدوران (fa-spin) للأيقونة
    this.classList.toggle("fa-spin");

    // إظهار أو إخفاء صندوق الإعدادات الجانبي بتحريكه عبر فئة (open)
    document.querySelector(".setting-box").classList.toggle("open");
};

// =========================================================================
// 3. التبديل الفوري للألوان عند النقر (Switch Colors Immediately)
// =========================================================================
// تحديد جميع عناصر الدوائر اللونية في صندوق الإعدادات
const colorList = document.querySelectorAll(".color-list li");

// ربط حدث النقر (Click Event) بكل دائرة لونية
colorList.forEach(li => {
    li.addEventListener("click", (e) => {
        // قراءة قيمة اللون من الخاصية المخصصة (data-color)
        const selectedColor = e.target.dataset.color;

        // 1. تطبيق اللون مباشرة على متغير الـ CSS الجذري (--main--color) لتتغير كافة واجهات الموقع فوراً
        document.documentElement.style.setProperty('--main--color', selectedColor);

        // 2. حفظ اللون المختار في الـ Local Storage لكي يستمر عند الانتقال بين الصفحات أو إعادة التحميل
        localStorage.setItem("color_option", selectedColor);

        // 3. تحديث لون خلفية أيقونة الترس (.im) وحاويتها (.toggle) لحظياً دون الحاجة لعمل تحديث للصفحة
        const imEl = document.querySelector(".setting-box .toggle .im");
        if (imEl) {
            imEl.style.setProperty("background-color", selectedColor);
        }
        const toggleEl = document.querySelector(".setting-box .toggle");
        if (toggleEl) {
            toggleEl.style.setProperty("background-color", selectedColor);
        }

        // نقل فئة (active) إلى العنصر الذي تم النقر عليه لتمييزه كخيار نشط
        handleActive(e);
    });
});

// Switch Random Background Option
const randomBackgroundsElement = document.querySelectorAll(".random-backgrounds span");

// Loop On All Spans
randomBackgroundsElement.forEach(span => {
    // Click On Every Span
    span.addEventListener("click", (e) => {
        handleActive(e);
        if (e.target.dataset.background === "yes") {
            backgroundOption = true;
            randomizeImgs();
            localStorage.setItem("background_option", true);
        }
        else {
            backgroundOption = false;
            clearInterval(backgroundInterval);
            localStorage.setItem("background_option", false);
        }
    });
});

// select landing page element
let landingPage = document.querySelector(".landing-page");
// Select Images Into Gallery
let imagesSource = document.querySelectorAll(".images-box img");
// Declearations
let featBoxImages = document.querySelectorAll(".feat-box img");
let personImages = document.querySelectorAll(".person-info img");
// get array of images
let imgsArray = ["g (1).jpg"];
// Add Images To The Array
for (let i = 1; i < 500; i++) {
    imgsArray.push(`g (${i}).jpg`);
}

landingPage.style.backgroundImage = `url("../img/${imgsArray[Math.floor(Math.random() * imgsArray.length)]}")`;

featBoxImages.forEach(img => {
    img.src = `../img/${imgsArray[Math.floor(Math.random() * imgsArray.length)]}`;
   
});
personImages.forEach(img => {
    img.src = `../img/${imgsArray[Math.floor(Math.random() * imgsArray.length)]}`;
});

imagesSource.forEach(img => {
    img.src = `../img/${imgsArray[Math.floor(Math.random() * imgsArray.length)]}`
});

// function to Randomize Imgs
function randomizeImgs() {
    if (backgroundOption === true) {
        backgroundInterval = setInterval(() => {
            // get random number
            let randomNumber = Math.floor(Math.random() * imgsArray.length);

            // change background url
            landingPage.style.backgroundImage = `url("../img/${imgsArray[Math.floor(Math.random() * imgsArray.length)]}")`;
            // Loop On Images Box To Set The Images Sources
            for (let i = 0; i < imagesSource.length; i++) {

                imagesSource[i].setAttribute("src", `../img/${imgsArray[Math.floor(Math.random() * imgsArray.length)]}`);
            }
        }, 4000)
    }
}

let im = document.querySelector("img");
im.setAttribute("src", `../img/${imgsArray[Math.floor(Math.random() * imgsArray.length)]}`);


randomizeImgs();

// Select Skills Selector 
let ourSkills = document.querySelector(".skills");
window.onscroll = function () {
    // Skills Offset Top
    let skillsOffsetTop = ourSkills.offsetTop;

    // Skills Outer Height
    let skillsOuterHeight = ourSkills.offsetHeight;

    // Window Height
    let windowHeight = this.innerHeight;

    // Window Scroll Top
    let windowScrollTop = this.pageYOffset;

    if (windowScrollTop > (skillsOffsetTop + skillsOuterHeight - windowHeight)) {
        let allSkills = document.querySelectorAll(".skill-box .skill-progress span");
        allSkills.forEach(skill => {

            skill.style.width = skill.dataset.progress;
        });
    }
};

// Create Popup With The Image
let ourGallery = document.querySelectorAll(".gallery img");

ourGallery.forEach(img => {

    img.addEventListener('click', (e) => {

        // Create Overlay Element
        let overlay = document.createElement("div");

        // Add Class To Overlay
        overlay.className = "popup-overlay";

        // Append Overlay To The Body
        document.body.appendChild(overlay);

        // Create The Popup Box
        let popupBox = document.createElement("div");

        // Add Class To The Popup Box
        popupBox.className = 'popup-box';

        if (img.alt !== null) {

            // Create Heading
            let imageHeading = document.createElement("h3");

            // Create Text For Heading
            let imageText = document.createTextNode(img.alt);

            // Append The Text To The Heading
            imageHeading.appendChild(imageText);

            // Append The Heading To The Popup Box
            popupBox.appendChild(imageHeading);
        }

        // Creat The Image
        let popupImage = document.createElement("img");

        // Set Image Source
        popupImage.src = img.src;

        // Add Image To Popup Box
        popupBox.appendChild(popupImage);

        // Append Popup Box To Body
        document.body.appendChild(popupBox);

        // Create The Close Span
        let closeButton = document.createElement("span");

        // Create The Close Button Text
        let closeButtonText = document.createTextNode("X");

        // Append Text To Close Button
        closeButton.appendChild(closeButtonText);

        // Add Class To The Close Button
        closeButton.className = 'close-button';

        // Add Close Button To The Popup Box
        popupBox.appendChild(closeButton);
    });
});

// Close The Popup Box
document.addEventListener('click', function (e) {

    if (e.target.className == 'close-button') {

        // Remove The Current Popup Box
        e.target.parentElement.remove();

        // Remove Overlay
        document.querySelector(".popup-overlay").remove();
    }
});

// Select All Bullets
const allBullets = document.querySelectorAll(".nav-bullets .bullet");

// Select All Links
const allLinks = document.querySelectorAll(".links a");

function scrollToSomeWhere(elements) {
    const allLinks = document.querySelectorAll(".links a");
    elements.forEach(element => {

        element.addEventListener('click', (e) => {
            e.preventDefault();
            document.querySelector(e.target.dataset.section).scrollIntoView({

                behavior: 'smooth'
            });
        });
    });
}

scrollToSomeWhere(allLinks);
scrollToSomeWhere(allBullets);

document.querySelector(".contact").style.backgroundImage = `url("../img/${imgsArray[Math.floor(Math.random() * imgsArray.length)]}")`;

// Handle Active State
function handleActive(ev) {
    // Remove Active Class From All Children
    ev.target.parentElement.querySelectorAll(".active").forEach(element => {
        element.classList.remove("active");
    });

    // Add Active Class On Self
    ev.target.classList.add("active");
}

let bulletsSpan = document.querySelectorAll(".bullets-option span");
let bulletsContainer = document.querySelector(".nav-bullets");
let bulletLocalItem = localStorage.getItem("bullets_option");

if (bulletLocalItem !== null) {

    bulletsSpan.forEach(span => {

        span.classList.remove('active');
    });
    if (bulletLocalItem === 'block') {
        bulletsContainer.style.display = "block";
        document.querySelector(".bullets-option .yes").classList.add('active');
    }
    else {
        bulletsContainer.style.display = "none";
        document.querySelector(".bullets-option .no").classList.add('active');
    }
}

bulletsSpan.forEach(span => {
    span.addEventListener('click', (e) => {

        if (e.target.dataset.display === "show") {
            bulletsContainer.style.display = "block";
            localStorage.setItem("bullets_option", "block");
        }
        else {
            bulletsContainer.style.display = "none";
            localStorage.setItem("bullets_option", "none");
        }

        handleActive(e);
    });
});

// Reset Buttoon
document.querySelector(".reset-options").onclick = function () {
    localStorage.removeItem("color_option");
    localStorage.removeItem("background_option");
    localStorage.removeItem("bullets_option");
    window.location.reload();
}
