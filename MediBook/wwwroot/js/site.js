// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Global Page Loader Logic
document.addEventListener("DOMContentLoaded", function () {
    const loader = document.getElementById("page-loader");
    const loaderBar = document.getElementById("loader-bar");

    let progress = 0;
    
    function finishLoader() {
        if (window.loaderInterval) {
            clearInterval(window.loaderInterval);
        }
        
        if (loader && loaderBar) {
            loaderBar.style.width = "100%";
            
            setTimeout(() => {
                loader.classList.remove("active");
                // Reset for next navigation
                setTimeout(() => {
                    loaderBar.style.width = "0%";
                }, 400);
            }, 300);
        }
    }

    function startLoader() {
        if (loader && loaderBar) {
            loader.classList.add("active");
            progress = 0;
            loaderBar.style.width = "0%";
            
            window.loaderInterval = setInterval(() => {
                progress += Math.random() * 15;
                if (progress > 90) progress = 90;
                
                loaderBar.style.width = `${progress}%`;
            }, 100);
        }
    }

    // Finish loader when page is fully loaded
    window.addEventListener("load", finishLoader);
    
    // If DOM is ready and window is already loaded (fallback)
    if (document.readyState === 'complete') {
        finishLoader();
    }

    // Intercept form submissions
    document.addEventListener("submit", function (e) {
        startLoader();
    });
    
    // Handle back/forward cache
    window.addEventListener("pageshow", function (e) {
        if (e.persisted) {
            finishLoader();
        }
    });

    // Initialize Global Toasts
    var toastElList = [].slice.call(document.querySelectorAll('.toast'));
    var toastList = toastElList.map(function (toastEl) {
        return new bootstrap.Toast(toastEl, { autohide: true, delay: parseInt(toastEl.getAttribute('data-bs-delay') || '4000') });
    });
    toastList.forEach(toast => toast.show());
});
