// DOM Elements
const navLinks = document.querySelectorAll('.nav-link');
const sections = document.querySelectorAll('section, header');
const skillCards = document.querySelectorAll('.skill-card');
const progressBars = document.querySelectorAll('.progress');
const contactBtns = document.querySelectorAll('.contact-btn');
const welcomeBtn = document.getElementById('welcomeBtn');
const themeBtn = document.getElementById('themeBtn');

// Theme colors for background
const themes = [
    { name: 'light', bg: '#ffffff', text: '#333333' },
    { name: 'dark', bg: '#1a1a1a', text: '#ffffff' },
    { name: 'blue', bg: '#e3f2fd', text: '#0d47a1' },
    { name: 'green', bg: '#e8f5e9', text: '#1b5e20' },
    { name: 'purple', bg: '#f3e5f5', text: '#4a148c' }
];

let currentThemeIndex = 0;

// Welcome button functionality
welcomeBtn.addEventListener('click', () => {
    const userName = prompt('Xin chào! Tên của bạn là gì?');
    
    if (userName && userName.trim()) {
        // Show alert
        alert(`Chào mừng ${userName.trim()} đến với portfolio của tôi! 🎉`);
        
        // Create floating message
        const floatingMsg = document.createElement('div');
        floatingMsg.textContent = `Chào ${userName.trim()}! 👋`;
        floatingMsg.style.cssText = `
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 1.5rem 3rem;
            border-radius: 50px;
            font-size: 1.5rem;
            font-weight: bold;
            z-index: 1000;
            box-shadow: 0 10px 40px rgba(0, 0, 0, 0.3);
            animation: popIn 0.5s ease-out forwards;
        `;
        
        document.body.appendChild(floatingMsg);
        
        // Remove after 2 seconds
        setTimeout(() => {
            floatingMsg.style.animation = 'popOut 0.5s ease-out forwards';
            setTimeout(() => floatingMsg.remove(), 500);
        }, 2000);
    }
});

// Theme button functionality
themeBtn.addEventListener('click', () => {
    currentThemeIndex = (currentThemeIndex + 1) % themes.length;
    const currentTheme = themes[currentThemeIndex];
    
    // Change background and text color
    document.body.style.backgroundColor = currentTheme.bg;
    document.body.style.color = currentTheme.text;
    
    // Update theme button icon and text based on theme
    const themeNames = ['☀️', '🌙', '🔵', '🟢', '🟣'];
    themeBtn.innerHTML = `<i class="fas fa-palette"></i> ${themeNames[currentThemeIndex]}`;
    
    // Add animation
    themeBtn.style.animation = 'spin 0.6s ease-out';
    setTimeout(() => {
        themeBtn.style.animation = 'none';
    }, 600);
    
    // Show notification
    const notification = document.createElement('div');
    notification.textContent = `Chế độ: ${currentTheme.name.charAt(0).toUpperCase() + currentTheme.name.slice(1)}`;
    notification.style.cssText = `
        position: fixed;
        bottom: 2rem;
        left: 2rem;
        background: ${currentTheme.bg};
        color: ${currentTheme.text};
        border: 2px solid ${currentTheme.text};
        padding: 1rem 1.5rem;
        border-radius: 10px;
        font-weight: bold;
        z-index: 999;
        animation: slideIn 0.3s ease-out;
        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
    `;
    
    document.body.appendChild(notification);
    
    setTimeout(() => {
        notification.style.animation = 'slideOut 0.3s ease-out forwards';
        setTimeout(() => notification.remove(), 300);
    }, 2000);
    
    // Save theme preference
    localStorage.setItem('preferredTheme', currentThemeIndex);
});

// Load saved theme on page load
window.addEventListener('load', () => {
    const savedTheme = localStorage.getItem('preferredTheme');
    if (savedTheme !== null) {
        currentThemeIndex = parseInt(savedTheme);
        const currentTheme = themes[currentThemeIndex];
        document.body.style.backgroundColor = currentTheme.bg;
        document.body.style.color = currentTheme.text;
    }
});

// Active navigation link on scroll
window.addEventListener('scroll', () => {
    let current = '';
    
    sections.forEach(section => {
        const sectionTop = section.offsetTop;
        const sectionHeight = section.clientHeight;
        
        if (window.pageYOffset >= sectionTop - 200) {
            current = section.getAttribute('id');
        }
    });

    navLinks.forEach(link => {
        link.classList.remove('active');
        if (link.getAttribute('href').slice(1) === current) {
            link.classList.add('active');
        }
    });
});

// Smooth scroll for nav links
navLinks.forEach(link => {
    link.addEventListener('click', (e) => {
        e.preventDefault();
        const targetId = link.getAttribute('href').slice(1);
        const targetSection = document.getElementById(targetId);
        
        if (targetSection) {
            targetSection.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// Animate skill cards on scroll
const observerOptions = {
    threshold: 0.1,
    rootMargin: '0px 0px -100px 0px'
};

const observer = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            entry.target.style.animation = 'fadeInUp 0.6s ease-out forwards';
            observer.unobserve(entry.target);
        }
    });
}, observerOptions);

skillCards.forEach(card => {
    observer.observe(card);
});

// Animate progress bars
const progressObserver = new IntersectionObserver((entries) => {
    entries.forEach(entry => {
        if (entry.isIntersecting) {
            const progress = entry.target;
            const width = progress.style.width;
            progress.style.width = '0';
            
            setTimeout(() => {
                progress.style.width = width;
            }, 100);
            
            progressObserver.unobserve(entry.target);
        }
    });
}, observerOptions);

progressBars.forEach(bar => {
    progressObserver.observe(bar);
});

// Add hover effect to contact buttons
contactBtns.forEach(btn => {
    btn.addEventListener('mouseenter', function() {
        this.style.transform = 'translateY(-5px)';
    });
    
    btn.addEventListener('mouseleave', function() {
        this.style.transform = 'translateY(0)';
    });
});

// Add active class to nav links on scroll (CSS support)
const style = document.createElement('style');
style.textContent = `
    .nav-link.active {
        color: #ffd700 !important;
    }
    
    .nav-link.active::after {
        width: 100% !important;
    }
`;
document.head.appendChild(style);

// Scroll to top button
const scrollToTopBtn = document.createElement('button');
scrollToTopBtn.innerHTML = '<i class="fas fa-arrow-up"></i>';
scrollToTopBtn.className = 'scroll-to-top';
scrollToTopBtn.style.cssText = `
    position: fixed;
    bottom: 2rem;
    right: 2rem;
    width: 50px;
    height: 50px;
    border-radius: 50%;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
    border: none;
    cursor: pointer;
    display: none;
    align-items: center;
    justify-content: center;
    font-size: 1.5rem;
    box-shadow: 0 4px 15px rgba(0, 0, 0, 0.2);
    z-index: 99;
    transition: all 0.3s ease;
`;

document.body.appendChild(scrollToTopBtn);

// Show/hide scroll to top button
window.addEventListener('scroll', () => {
    if (window.pageYOffset > 300) {
        scrollToTopBtn.style.display = 'flex';
    } else {
        scrollToTopBtn.style.display = 'none';
    }
});

// Scroll to top on button click
scrollToTopBtn.addEventListener('click', () => {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
});

// Hover effect for scroll to top button
scrollToTopBtn.addEventListener('mouseenter', function() {
    this.style.transform = 'scale(1.1)';
    this.style.boxShadow = '0 6px 20px rgba(0, 0, 0, 0.3)';
});

scrollToTopBtn.addEventListener('mouseleave', function() {
    this.style.transform = 'scale(1)';
    this.style.boxShadow = '0 4px 15px rgba(0, 0, 0, 0.2)';
});

// Load animation on page load
window.addEventListener('load', () => {
    document.body.style.opacity = '1';
});

// Console welcome message
console.log('%cWelcome to my portfolio!', 'color: #667eea; font-size: 20px; font-weight: bold;');
console.log('%cDesigned with ❤️ using HTML, CSS & JavaScript', 'color: #764ba2; font-size: 14px;');
