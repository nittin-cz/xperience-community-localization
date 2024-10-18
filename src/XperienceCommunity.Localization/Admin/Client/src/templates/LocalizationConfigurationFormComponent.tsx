import Paper from '@mui/material/Paper';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import React, { useCallback, useEffect, useState } from 'react';

export interface LocalizationTranslationModel {
    id: number;
    languageId: string;
    languageName: string;
    translationText: string;
}
/* eslint-disable @typescript-eslint/naming-convention */
export interface LocalizationConfigurationComponentClientProperties {
    value: LocalizationTranslationModel[];
    onChange: (value: LocalizationTranslationModel[]) => void;
}

const TextAreaCellComponent = ({ value, onChange }: { value: string; onChange: (value: string) => void }): JSX.Element => {
    const [text, setText] = useState<string>(value);

    const handleChange = (event: React.ChangeEvent<HTMLTextAreaElement>): void => {
        const newValue = event.target.value;
        setText(newValue);
        onChange(newValue);
    };

    return (
        <div className="text-area-container___tzehk">
            <div className="text-area-wrapper___goToE">
                <textarea className="text-area___x0Rzd"
                    value={text}
                    onChange={handleChange}
                    style={{ width: '100%', height: '100px' }}
                />
            </div>
        </div>
    );
};
/* eslint-enable @typescript-eslint/naming-convention */

export const LocalizationConfigurationFormComponent = (
    props: LocalizationConfigurationComponentClientProperties,
): JSX.Element => {
    const [translations, setTranslations] = useState<LocalizationTranslationModel[]>(props.value);

    const handleTranslationChange = useCallback((translation: LocalizationTranslationModel, newValue: string): void => {
        const newTranslations = [...translations];
        const translationIndex = translations.findIndex((x: LocalizationTranslationModel) => {
            return x.languageId === translation.languageId; 
        })
        newTranslations[translationIndex].translationText = newValue;
        setTranslations(newTranslations);
            props.onChange(newTranslations);
    }, [translations, props]);

    useEffect(() => {
        if (props.value !== translations) {
            setTranslations(props.value);
        }
    }, [props.value, translations]);

    return (
        <TableContainer component={Paper}>
            <Table>
                <TableHead>
                    <TableRow>
                        <TableCell>Language</TableCell>
                        <TableCell>Translation</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {translations.map(translation => (
                        <TableRow key={translation.languageId}>
                            <TableCell>{translation.languageName}</TableCell>
                            <TableCell>
                                <TextAreaCellComponent
                                    value={translation.translationText}
                                    onChange={(newValue: string) => { handleTranslationChange(translation, newValue) }}/>
                            </TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    );
};
