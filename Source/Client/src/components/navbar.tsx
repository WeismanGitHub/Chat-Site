export default function Navbar() {
    return <nav className="navbar navbar-expand navbar-dark bg-primary ps-2 pe-2 justify-content-center py-1">
        <a className="navbar-brand" href="/">
            <img src="/icon.png" width={50} height={50} alt="icon" className="me-2"/>
            Chat Site v2
        </a>

        <div className="justify-content-start navbar-nav">
            <a className="nav-item nav-link active" href="/">Home</a>
            <a className="nav-item nav-link active" href="/about">About</a>
            <a className="nav-item nav-link active" href="/auth">Signin/Signup</a>
        </div>
    </nav>
}