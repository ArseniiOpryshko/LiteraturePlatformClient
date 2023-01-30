let main = document.querySelector('.mainparttext');
let pages = document.querySelector('.pages');
let text = main.innerHTML;
let lenght = text.length;
let textArray = [];
let row = 0;

for (let i = 0; i < text.length / 2210; i++) {
    textArray[i] = text.substring(row, row + 2210);
    row += 2100; 
    pages.innerHTML+="<div data-index=\"" + (i) + "\" class=\"number\"><span>" + (i+1) + "</span></div>";
}
main.innerHTML=textArray[0];

let numbers = document.querySelectorAll('.number');
numbers[0].classList.add('clicked');
numbers.forEach(el => {
    el.addEventListener('click', ()=>{
        numbers.forEach(elem => {
            elem.classList.remove('clicked');
        });
        el.classList.add('clicked');
        main.innerHTML=textArray[el.dataset.index];
    });
});


