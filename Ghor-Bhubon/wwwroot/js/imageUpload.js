document.addEventListener("DOMContentLoaded", function () {
    const imageInput = document.getElementById("imageInput");
    const imageContainer = document.getElementById("imageContainer");

    imageInput.addEventListener("change", function (event) {
        const files = event.target.files;
        if (files.length === 0) return;

        for (let file of files) {
            const reader = new FileReader();
            reader.onload = function (e) {
                const imageWrapper = document.createElement("div");
                imageWrapper.classList.add("image-wrapper");

                const img = document.createElement("img");
                img.src = e.target.result;

                const removeBtn = document.createElement("button");
                removeBtn.innerHTML = "×";
                removeBtn.classList.add("remove-btn");
                removeBtn.onclick = function () {
                    imageWrapper.remove();
                };

                imageWrapper.appendChild(img);
                imageWrapper.appendChild(removeBtn);
                imageContainer.appendChild(imageWrapper);
            };
            reader.readAsDataURL(file);
        }
    });
});
