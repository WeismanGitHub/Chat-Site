import { ToastContainer, Toast, Modal, Button } from 'react-bootstrap';
import { useQuery } from '@tanstack/react-query';
import Endpoints from '../../endpoints';
import { useEffect, useState } from 'react';
import axios, { AxiosError } from 'axios';

export default function Friends() {
    // eslint-disable-next-line prefer-const
    let { error, data } = useQuery<Friend[], AxiosError<APIErrorRes<object>>>({
        queryKey: ['data'],
        queryFn: () => Endpoints.Friends.get(),
    });

    const [toastError, setToastError] = useState<APIErrorRes<object> | null>(null);
    const [showError, setShowError] = useState(false);

    useEffect(() => {
        if (error) {
            if (axios.isAxiosError<APIErrorRes<object>>(error) && error.response?.data) {
                setToastError(error.response.data);
                setShowError(true);
                console.log(toastError);
            }
        }
    }, [error]);

    const [showModal, setShowModal] = useState(false);
    const [selectedFriend, setFriend] = useState<Friend | null>(null);

    async function removeFriend() {
        try {
            await Endpoints.Friends.remove(selectedFriend!.id);
            data = data!.filter((friend) => friend.id === selectedFriend?.id);
            setFriend(null);
            setShowModal(false);
        } catch (err: unknown) {
            if (axios.isAxiosError<APIErrorRes<object>>(error) && error.response?.data) {
                setToastError(error.response.data);
                setShowError(true);
                console.log(toastError);
            }
        }
    }

    return (
        <div className="text-center">
            <ToastContainer position="top-end">
                <Toast
                    onClose={() => setShowError(!showError)}
                    show={showError}
                    autohide={true}
                    className="d-inline-block m-1"
                    bg={'danger'}
                >
                    <Toast.Header>
                        <strong className="me-auto">
                            {toastError?.message || 'Unable to read error name.'}
                        </strong>
                    </Toast.Header>
                    <Toast.Body>
                        {toastError?.errors &&
                            Object.values(toastError?.errors).map((err) => {
                                return <div key={err}>{err}</div>;
                            })}
                    </Toast.Body>
                </Toast>
            </ToastContainer>

            <Modal show={showModal}>
                <Modal.Dialog>
                    <Modal.Header
                        closeButton
                        onClick={() => {
                            setShowModal(false);
                            setFriend(null);
                        }}
                    >
                        <Modal.Title>Friend</Modal.Title>
                    </Modal.Header>

                    <Modal.Body>
                        {selectedFriend?.displayName || "Could not get friend's name."}
                        <div className="fs-6">
                            Created -{' '}
                            {!selectedFriend
                                ? 'Unkown'
                                : new Date(selectedFriend.createdAt).toLocaleDateString('en-US', {
                                      weekday: 'long',
                                      year: 'numeric',
                                      month: 'long',
                                      day: 'numeric',
                                  })}
                        </div>
                    </Modal.Body>

                    <Modal.Footer>
                        <Button variant="danger" onClick={removeFriend}>
                            Remove
                        </Button>
                    </Modal.Footer>
                </Modal.Dialog>
            </Modal>

            <ul className="list-group fs-5">
                {data?.length
                    ? data.map((friend) => {
                          return (
                              <li
                                  className="list-group-item bg-dark-subtle text-primary border-secondary"
                                  key={friend.id}
                                  style={{ cursor: 'pointer' }}
                                  onClick={() => {
                                      setFriend(friend);
                                      setShowModal(true);
                                  }}
                              >
                                  {friend.displayName}
                                  <div className="fs-6">
                                      Created -{' '}
                                      {new Date(friend.createdAt).toLocaleDateString('en-US', {
                                          weekday: 'long',
                                          year: 'numeric',
                                          month: 'long',
                                          day: 'numeric',
                                      })}
                                  </div>
                              </li>
                          );
                      })
                    : 'No Friends'}
            </ul>
        </div>
    );
}
