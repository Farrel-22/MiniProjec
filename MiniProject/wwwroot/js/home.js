document.addEventListener('DOMContentLoaded', async () => {
    try {
        const user = await MiniApp.requireAuth();
        document.getElementById('welcomeText').textContent = 'Welcome, ' + (user.fullName || user.username);
    } catch (err) {
        // ignore: api already redirects on 401
    }
});
