/* eslint-disable react/no-unescaped-entities */
import Navbar from '../components/navbar';

export default function About() {
    return (
        <>
            <Navbar />
            <div className="container w-50">
                <div className="row vh-100 align-items-center justify-content-center">
                    <div className="text-center bg-white rounded shadow card-body p-3 bg-primary">
                        <h1 className="mb-2">Chat Site v2</h1>
                        <h5 className="mb-2">
                            <a
                                href="https://github.com/WeismanGitHub/Chat-Site-v2"
                                className="link-underline-primary"
                            >
                                Chat Site v2 Github
                            </a>
                        </h5>
                        <h5 className="mb-2">
                            <a
                                href="https://github.com/WeismanGitHub/Chat-Website"
                                className="link-underline-primary"
                            >
                                Chat Site v1 Github
                            </a>
                        </h5>
                        <br />
                        <a
                            href="/swagger"
                            className="btn btn-primary btn-lg"
                            role="button"
                        >
                            View API With Swagger
                        </a>
                    </div>
                    <div className="bg-white rounded shadow card-body p-3 text-center fs-5 bg-primary">
                        <p>
                            This website provides users with a platform for
                            connecting, adding friends, and engaging in
                            real-time one-on-one or group chats, among other
                            features.
                        </p>
                        <p>
                            I'm revisiting a{' '}
                            <a
                                href="https://github.com/WeismanGitHub/Chat-Website"
                                className="link-underline-primary"
                            >
                                prior project
                            </a>{' '}
                            that I developed with <strong>JavaScript</strong>.
                            However, this time, I've strategically chosen to
                            harness the capabilities of <strong>ASP.NET</strong>{' '}
                            and the <strong>FastEndpoints</strong> library,
                            ensuring a robust and scalable infrastructure.
                            Additionally, with a powerful combination of{' '}
                            <strong>Typescript, React, and Bootstrap</strong>,
                            I've created a well-designed and easy to use
                            front-end.
                        </p>
                        <p>
                            The driving force behind this endeavor is my
                            unwavering commitment to elevating my C# programming
                            skills to new levels of mastery. By revisiting and
                            enhancing my prior work, I aim to not only reinforce
                            my existing knowledge but also push the boundaries
                            of what I can achieve.
                        </p>
                    </div>
                </div>
            </div>
        </>
    );
}
