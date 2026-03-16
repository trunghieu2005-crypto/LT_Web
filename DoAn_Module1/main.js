function handleLogout() {
    // 1. Giả sử bạn lưu thông tin user trong LocalStorage
    localStorage.removeItem('userToken'); 
    localStorage.removeItem('username');

    // 2. Thông báo nhẹ một cái
    alert("Bạn đã đăng xuất thành công!");

    // 3. Chuyển hướng về trang chủ (index.html)
    window.location.reload();
}

// Hàm Đăng ký
async function handleRegister(username, password) {
    try {
        // 1. Gửi yêu cầu POST tới địa chỉ Server
        const response = await fetch('http://localhost:3000/api/register', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password }) // Đóng gói dữ liệu thành JSON
        });

        const data = await response.json(); // Đợi câu trả lời từ Server

        if (response.ok) {
            alert("Thành công: " + data.message);
            // Sau khi đăng ký xong, có thể chuyển sang trang đăng nhập
        } else {
            alert("Lỗi: " + data.message);
        }
    } catch (error) {
        console.error("Không kết nối được tới Server:", error);
    }
}

// Hàm Đăng nhập
async function handleLogin(username, password) {
    try {
        const response = await fetch('http://localhost:3000/api/login', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, password })
        });

        const data = await response.json();

        if (response.ok) {
            alert(data.message);
            // Lưu trạng thái đăng nhập vào máy khách
            localStorage.setItem('isLoggedIn', 'true');
            localStorage.setItem('username', username);
            window.location.href = 'index.html'; // Về trang chủ
        } else {
            alert("Thất bại: " + data.message);
        }
    } catch (error) {
        alert("Server chưa bật ông giáo ạ!");
    }
}