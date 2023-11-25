type APIErrorRes<APIError> = {
    errors: APIError & { generalErrors?: string };
    message: string;
    statusCode: number;
};
