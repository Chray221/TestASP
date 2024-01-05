// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Shorthand for $( document ).ready()
// $(function()
// {
//     console.log("document is ready");
//     const elements = document.querySelector['[data-bs-toggle="toggle"]'];
//     $('[data-bs-toggle="toggle"]').click()
//     if(elements != null){
//         console.log(elements);
//         for(let element of elements){
//         }
//     }
// });

$('[data-bs-toggle="toggle"]').click(function (event) {
	let target = event.target.getAttribute('data-bs-target');
	let isToggled = event.target.getAttribute('aria-toggle') == 'true';

    if(event.target.getAttribute("type") == 'checkbox')
    {
        isToggled = event.target.checked;
    }
    if(!target) return;
    // if(isToggled)
    // {
    //     $(target).removeAttr('hidden');
    // }
    // else 
    // {
    //     $(target).attr('hidden','');
    // }
    for(let index = 0; index < $(target).length; index++){
        $(target)[index].toggleElement(isToggled);
    }
    event.target.setAttribute('aria-toggle', isToggled == true ? 'false' : 'true');
    
});


function boolean_choice_click(e,trueClass,falseClass){  
    for(let falseItem of document.getElementsByClassName(falseClass)){
        falseItem.toggleElement(e.value == "False");
    }
    for(let trueItem of document.getElementsByClassName(trueClass)){
        trueItem.toggleElement(e.value == "True");
    }
}

Element.prototype.toggleElement  = function toggleElement( isShow)
{
    console.log(`${this.className}: isShowing? ${isShow}`);
    isShow = isShow ?? !this.hasAttribute("hidden");
    if(!isShow){
        hideElement(this);
    }
    else {
        showElement(this);
    }
}

// Element.prototype.hideElement  = function hideElement()
// {
//     hideElement(this);
// }

// Element.prototype.showElement  = function showElement()
// {
//     showElement(this);
// }

/**
 * @param {Element} e The date
 */
function hideElement(e)
{
    e.setAttribute("hidden","");
}

/**
 * @param {Element} e The date
 */
function showElement(e)
{
    e.removeAttribute("hidden");
}
const SHOW_PASSWORD_ICON = "fa-regular fa-eye";
const HIDE_PASSWORD_ICON = "fa-regular fa-eye-slash";
function togglePassword(button,inputId)
{
    var input = document.getElementById(inputId);
    var isPassword = input.getAttribute("type") === "password";
    var icon = isPassword ? SHOW_PASSWORD_ICON : HIDE_PASSWORD_ICON;
    button.innerHTML = `<i class="${icon}"/>`;
    input.setAttribute("type", isPassword ? "text" : "password");
}

function toggle(checkbox,viewToHide)
{
    $(`#${viewToHide}`).toggleElement(checkbox.value);
}

function cloneTemplate(containerId,templateObj) {
    console.log(`container: #${containerId}`);        
    return $(`#${containerId}`).append(templateObj.html()).children(':last');
}

function setInputGroup(parent, index, id) {
    var regexId = formatId(id);
    var inputTag = "input, textarea, select";
    parent.find('label').eq(index).attr("for",regexId);
    parent.find(inputTag).eq(index).attr("id",regexId);
    parent.find(inputTag).eq(index).attr("name",id);
    parent.find('span').eq(index).attr("data-valmsg-for",regexId);
}

function formatId(id) {
    const regex = /(?:[\[\].])/gi;
    return id.replaceAll(regex,"_") ;
}