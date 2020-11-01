var urlInput = document.querySelector("#urlshort");
var submitBtn = document.querySelector("#submit"); 
var respArea = document.querySelector("#resp-area");

submitBtn.onclick = function (ev)
{
    if (!this.classList.contains("copy")) {
        var url = urlInput.value;
        if(!validateURL(url)){
            RespAreaRed("Invalid URL");
            return null;
        }
        fetch("/",{
            method:"POST",
            body: JSON.stringify(url), 
            headers:{
                'Content-Type': 'application/json'
            }
        }).then(res => res.json())
        .then(response => {
            if (response.status === "URL already exists") {
                console.log(response);
                urlInput.value = new URL(window.location) + response.token;
                RespAreaGreen("URL already exists");
				submitBtn.innerHTML = "Copy";
                submitBtn.classList.add("copy");
            }
			else if (response.status === "already shortened") {
				urlInput.value = "";
                RespAreaRed("That link has already been shortened");
            }
            else {
                console.log(response);
                urlInput.value = new URL(window.location) + response;
                submitBtn.innerHTML = "Copy";
                submitBtn.classList.add("copy");
            }
        })
        .catch(error => console.log(error));
    }
    else {
        urlInput.select();
        document.execCommand("copy");
    }

    if (submitBtn.innerHTML === "Reload") {
        location.reload();
    }
    else if (submitBtn.innerHTML === "Copy") {
        submitBtn.innerHTML = "Reload";
    }
}

function validateURL(url){
    var pattern = new RegExp('https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,}'); 
  return !!pattern.test(url);
}

respArea.addEventListener("mouseout", function(ev){
    setTimeout(() => {
        respArea.classList.remove("active-red");
        respArea.classList.remove("active-green");
        respArea.classList.add("inactive");
    }, 2000);
    
})

respArea.onclick = function(ev){
    respArea.classList.remove("active-red");
    respArea.classList.remove("active-green");
    respArea.classList.add("inactive");
}

function RespAreaRed(text){
    respArea.classList.remove("inactive");
    respArea.classList.add("active-red");
    respArea.innerHTML = text;
}

function RespAreaGreen(text) {
    respArea.classList.remove("inactive");
    respArea.classList.add("active-green");
    respArea.innerHTML = text;
}