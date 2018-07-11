export class HttpService {

    private static performHttpRequest<T>(
        method: string,
        url: string,
        data?: { [key: string]: any },
        files?: FileList,
        successCallback?: (data: T) => void,
        errorCallback?: (status: number, responseText: string) => void
    ) {
        const formData = data || files ? new FormData() : null;

        if (data) {
            for (let key in data) {
                if (data.hasOwnProperty(key)) {
                    if (formData) {
                        formData.append(key, data[key]);
                    }
                }
            }
        }

        if (files) {
            for (let i = 0; i < files.length; i++) {
                if (formData) {
                    formData.append(files[i].name, files[i]);
                }
            }
        }

        const xhr = new XMLHttpRequest();
        xhr.open(method, url, true);
        xhr.onload = () => {
            if (xhr.status >= 200 && xhr.status < 300) {
                if (successCallback) {
                    const responseData = xhr.responseText ? JSON.parse(xhr.responseText) : null;
                    successCallback(responseData);
                }
            } else if (errorCallback) {
                errorCallback(xhr.status, xhr.responseText);
            }
        };
        xhr.send(formData);
    }

    public static get<T>(
        url: string,
        successCallback: (data: T) => void,
        errorCallback?: (status: number, responseText: string) => void
    ) {
        HttpService.performHttpRequest("GET", url, undefined, undefined, successCallback, errorCallback);
    }

    public static post<T>(
        url: string,
        data?: { [key: string]: any },
        files?: FileList,
        successCallback?: (data: T) => void,
        errorCallback?: (status: number, responseText: string) => void
    ) {
        HttpService.performHttpRequest("POST", url, data, files, successCallback, errorCallback);
    }
}