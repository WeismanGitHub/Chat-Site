import { Dispatch, SetStateAction, useEffect, useState } from 'react';
import { ToastContainer, Toast } from 'react-bootstrap';
import { useQuery } from '@tanstack/react-query';
import Endpoints from '../../endpoints';
import { HTTPError } from 'ky';

export default function Conversations({
    setConvoID,
    setConversations,
    conversations,
}: {
    setConvoID: Dispatch<SetStateAction<string | null>>;
    setConversations: Dispatch<SetStateAction<ConversationsData>>;
    conversations: ConversationsData;
}) {
    const { error, data } = useQuery<ConversationsData, HTTPError>({
        queryKey: ['data'],
        queryFn: () => Endpoints.Conversations.get(),
    });

    const [toastError, setToastError] = useState<APIErrorRes<object> | null>(null);
    const [showError, setShowError] = useState(false);
    setConversations(data ?? []);

    useEffect(() => {
        if (error) {
            error.response.json().then((res) => {
                console.error(res);
                setToastError(res);
                setShowError(true);
            });
        }
    }, [error]);

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

            <ul className="list-group fs-5">
                {conversations.length
                    ? conversations.map((convo) => {
                          return (
                              <li
                                  className="list-group-item bg-dark-subtle text-primary border-secondary"
                                  key={convo.id}
                                  style={{ cursor: 'pointer' }}
                                  onClick={() => {
                                      setConvoID(convo.id);
                                  }}
                              >
                                  {convo.name ?? 'Unknown'}
                                  <div className="fs-6">
                                      Created -{' '}
                                      {new Date(convo.createdAt).toLocaleDateString('en-US', {
                                          weekday: 'long',
                                          year: 'numeric',
                                          month: 'long',
                                          day: 'numeric',
                                      })}
                                  </div>
                              </li>
                          );
                      })
                    : 'No Conversations'}
            </ul>
        </div>
    );
}
