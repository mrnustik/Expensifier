export async function deleteAccount(id: string) {
    const response = await fetch(`/api/accounts/${id}`, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
        },
    });
    console.log(response);
}
