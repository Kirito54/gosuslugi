window.saveAsFile = (filename, bytesBase64) => {
    const link = document.createElement('a');
    link.download = filename;
    link.href = bytesBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};

window.signWithCryptoPro = async (base64) => {
    return new Promise((resolve, reject) => {
        const run = function* () {
            try {
                const oStore = yield cadesplugin.CreateObjectAsync("CAPICOM.Store");
                yield oStore.Open(cadesplugin.CAPICOM_CURRENT_USER_STORE, "MY", cadesplugin.CAPICOM_STORE_OPEN_MAXIMUM_ALLOWED);
                const certs = yield oStore.Certificates;
                const selected = yield certs.Select("Выбор сертификата", "Выберите сертификат для подписи", false);
                if (!selected || (yield selected.Count) === 0) throw new Error("Сертификат не выбран");
                const cert = yield selected.Item(1);

                const signer = yield cadesplugin.CreateObjectAsync("CAdESCOM.CPSigner");
                yield signer.propset_Certificate(cert);

                const sData = yield cadesplugin.CreateObjectAsync("CAdESCOM.CadesSignedData");
                yield sData.propset_ContentEncoding(cadesplugin.CADESCOM_BASE64_TO_BINARY);
                yield sData.propset_Content(base64);

                const signature = yield sData.SignCades(signer, cadesplugin.CADESCOM_CADES_BES, true);
                resolve(signature);
            } catch (err) {
                reject(err.message || err);
            }
        };
        cadesplugin.async_spawn(run);
    });
};

