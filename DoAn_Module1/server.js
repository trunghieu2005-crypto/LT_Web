const express = require('express');
const cors = require('cors');
const app = express();

app.use(cors());
app.use(express.json());

let users = [];

//API Register//
app.post('/api/register', (req, res) => {
    const { username, password } = req.body;
    if (users.find(user => user.username === username)) {
        return res.status(400).json({ message: 'Tên đăng nhập đã tồn tại' });
    }
    users.push({ username, password });
    res.status(201).json({ message: 'Đăng ký người dùng thành công' });
});

//API Login//
app.post('/api/login', (req, res) => {
    const { username, password } = req.body;
    if(users.find(user => user.username === username && user.password === password)) {
        return res.status(200).json({ message: 'Chào mừng bạn đến BoardGames-Arena' });
    }else {
        return res.status(400).json({ message: 'Tên đăng nhập hoặc mật khẩu không đúng' });
    }
});

const PORT = 3000;
app.listen(PORT, () => {
    console.log(`Server đang chạy trên cổng ${PORT}`);
});
