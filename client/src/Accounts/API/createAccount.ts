export async function createAccount(model: IAccountCreateModel) {
    const response = await fetch('/api/accounts/create', {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(model)
    });
    console.log(response);
}

export interface IAccountCreateModel {
    name: string
}