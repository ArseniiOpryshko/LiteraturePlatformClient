let main = document.querySelector('.mainparttext');
let pages = document.querySelector('.pages');
let text = "Feugiat mi ad magnis tincidunt scelerisque, adipiscing arcu aenean. Maximus dui consequat pretium mus hac at, facilisis nec suscipit commodo class egestas, tellus proin varius ultricies arcu. Senectus quam dictumst fames purus class nulla facilisi libero ipsum sodales, diam montes magnis metus rutrum cursus ullamcorper porta. Facilisi penatibus at vehicula leo suscipit nulla aliquet rhoncus elit lacinia, orci dignissim sit amet mus aenean himenaeos in molestie, euismod adipiscing auctor maecenas volutpat hendrerit sollicitudin venenatis convallis. Vestibulum mus bibendum posuere hac senectus vivamus iaculis, nibh ante ipsum phasellus dictum conubia aptent, feugiat tempor ligula semper magna libero. Finibus pretium pellentesque aenean mauris sem praesent taciti metus phasellus, quis adipiscing per tempus nisl fringilla semper lobortis, sodales sagittis dictumst maximus fames odio duis justo Habitasse proin taciti aliquet purus sit ultrices nisi eros porta egestas efficitur, risus mi pellentesque dignissim dui ex fames tincidunt laoreet sem vehicula, placerat elit montes nascetur imperdiet bibendum scelerisque consequat ad ipsum. Litora laoreet mauris luctus praesent sem ipsum bibendum sodales, maximus nisl ultricies ac sed sit vulputate accumsan, vehicula malesuada condimentum leo consequat habitasse ad. At elit aliquam efficitur mus dolor vitae tempor arcu aliquet duis, tincidunt felis magna himenaeos aenean pretium non posuere. Interdum sagittis sit turpis taciti faucibus volutpat, id condimentum integer ipsum lectus efficitur, ligula vitae finibus dignissim magnis. Faucibus posuere ullamcorper lacus ultricies nulla fermentum laoreet, finibus vestibulum quis vivamus pretium praesent nunc, tempus felis dapibus habitasse est sagittis. Senectus laoreet aliquam tempus quisque natoque eget quis condimentum primis, velit est maximus nunc facilisi egestas vehicula etiam bibendum posuere, curae potenti gravida vestibulum vitae sodales sagittis nullam. Ipsum enim lorem ante metus erat ligula sollicitudin purus, ac dis netus volutpat porttitor euismod. Morbi primis ac luctus gravida non sit volutpat accumsan dui, lorem sed proin auctor malesuada aliquet quis fermentum viverra interdum, metus nisl diam risus lacus adipiscing euismod vestibulum. Nisl magnis amet nibh ultricies enim feugiat curabitur, lacinia bibendum habitasse scelerisque velit lectus, tortor ante non aliquam dictum eros. Dolor curabitur imperdiet eros pellentesque arcu ad hac nam inceptos, fames dui porttitor hendrerit diam semper eu proin euismod ut, sed congue lectus dictumst egestas rhoncus fusce at. Quisque amet semper feugiat integer hendrerit bibendum maecenas malesuada class, natoque rutrum hac taciti ipsum dis fusce eleifend ex, ullamcorper viverra dictumst netus nullam est dui mauris. Molestie massa aptent tellus nulla congue sociosqu ipsum, lorem suscipit elementum leo amet tincidunt, venenatis eu potenti donec iaculis consequat tate parturient justo diam ornare litora elementum fames, congue quis erat vivamus nascetur faucibus tincidunt consequat, sed rhoncus risus nisl id porta. Quam pellentesque convallis curae sed suscipit nostra libero auctor ligula, potenti placerat dis ante taciti magna arcu efficitur adipiscing proin, consectetur cursus nunc neque himenaeos turpis phasellus maecenas. Facilisis mi nulla ex dictumst neque dictum morbi condimentum mus sapien finibus eget, torquent vitae varius sociosqu hendrerit maecenas nunc congue lacus placerat lobortis, nisi penatibus ad metus ligula blandit integer habitasse justo proin vehicul";
let lenght = text.length;
let textArray = [];
let row = 0;

for (let i = 0; i < text.length/2300; i++) {
    textArray[i] = text.substring(row, row+2300);
    row+=2300; 
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


