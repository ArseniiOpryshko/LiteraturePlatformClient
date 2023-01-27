const dropdowns = document.querySelectorAll('.dropdown');
dropdowns.forEach(element => {
    const select = element.querySelector('.select');
    const caret = element.querySelector('.caret');
    const menu = element.querySelector('.menu');
    const options = element.querySelectorAll('.menu li');
    const selected = element.querySelector('.selected');

    select.addEventListener('click', ()=>{
        select.classList.toggle('select-clicked');
        caret.classList.toggle('caret-rotate');
        menu.classList.toggle('menu-open');
    });
    options.forEach(option=>{
        option.addEventListener('click', ()=>{
            // selected.innerText = option.innerText;
            select.classList.remove('select-clicked');
            caret.classList.remove('caret-rotate');
            menu.classList.remove('menu-open');
            options.forEach(opt=>{
                opt.classList.remove('active');
            });
            option.classList.add('active');
        });
    });
});