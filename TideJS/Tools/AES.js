import { base64ToBytes, bytesToBase64 } from "./Utils.js";

/**
 * Credits to https://github.com/bradyjoslin for the below AES implementation
 */
const enc = new TextEncoder();
const dec = new TextDecoder();

const getPasswordKey = (password) =>
    window.crypto.subtle.importKey("raw", enc.encode(password), "PBKDF2", false, [
        "deriveKey",
]);

const deriveKey = (passwordKey, salt, keyUsage) =>
    window.crypto.subtle.deriveKey(
        {
            name: "PBKDF2",
            salt: salt,
            iterations: 250000,
            hash: "SHA-256",
        },
            passwordKey,
        { name: "AES-GCM", length: 256 },
        false,
        keyUsage
);

/**
 * @param {string} secretData 
 * @param {string} password 
 * @returns 
 */
export async function encryptData(secretData, password) {
    try {
        const salt = window.crypto.getRandomValues(new Uint8Array(16));
        const iv = window.crypto.getRandomValues(new Uint8Array(12));
        const passwordKey = await getPasswordKey(password);
        const aesKey = await deriveKey(passwordKey, salt, ["encrypt"]);
        const encryptedContent = await window.crypto.subtle.encrypt(
        {
            name: "AES-GCM",
            iv: iv,
        },
        aesKey,
        enc.encode(secretData)
        );

        const encryptedContentArr = new Uint8Array(encryptedContent);
        let buff = new Uint8Array(
            salt.byteLength + iv.byteLength + encryptedContentArr.byteLength
        );
        buff.set(salt, 0);
        buff.set(iv, salt.byteLength);
        buff.set(encryptedContentArr, salt.byteLength + iv.byteLength);
        const base64Buff = bytesToBase64(buff);
        return base64Buff;
    } catch (e) {
        console.log(`Error - ${e}`);
        return "";
    }
}

/**
 * @param {string} encryptedData 
 * @param {string} password 
 * @returns 
 */
export async function decryptData(encryptedData, password) {
    try {
        const encryptedDataBuff = base64ToBytes(encryptedData);
        const salt = encryptedDataBuff.slice(0, 16);
        const iv = encryptedDataBuff.slice(16, 16 + 12);
        const data = encryptedDataBuff.slice(16 + 12);
        const passwordKey = await getPasswordKey(password);
        const aesKey = await deriveKey(passwordKey, salt, ["decrypt"]);
        const decryptedContent = await window.crypto.subtle.decrypt(
        {
            name: "AES-GCM",
            iv: iv,
        },
        aesKey,
        data
        );
        return dec.decode(decryptedContent);
    } catch (e) {
        console.log(`Error - ${e}`);
        return "Decryption Failed";
    }
}