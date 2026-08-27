import Tooltip from '@mui/material/Tooltip';
import React from 'react';
import { MdCheckCircle, MdError, MdWarning } from 'react-icons/md';

export type TranslationStatus = 'complete' | 'partial' | 'none';

export interface TranslationPreview {
    languageName: string;
    text: string;
}

export interface TranslationStatusTableCellComponentProps {
    status: TranslationStatus;
    missingLanguageNames: string[];
    translations: TranslationPreview[];
}

const statusColorByStatus: Record<TranslationStatus, string> = {
    complete: '#2e7d32',
    partial: '#ed6c02',
    none: '#d32f2f',
};

const StatusIcon = ({ status }: { status: TranslationStatus }): JSX.Element => {
    const color = statusColorByStatus[status];

    if (status === 'complete') {
        return <MdCheckCircle color={color} size={18} />;
    }

    if (status === 'partial') {
        return <MdWarning color={color} size={18} />;
    }

    return <MdError color={color} size={18} />;
};

const TooltipContent = (props: TranslationStatusTableCellComponentProps): JSX.Element => {
    if (props.status === 'none') {
        return <span>No translations</span>;
    }

    return (
        <span>
            {props.translations.map(translation => (
                <div key={translation.languageName}>
                    <strong>{translation.languageName}:</strong> {translation.text}
                </div>
            ))}
            {props.missingLanguageNames.length > 0 && (
                <div>Missing: {props.missingLanguageNames.join(', ')}</div>
            )}
        </span>
    );
};

export const TranslationStatusTableCellComponent = (
    props: TranslationStatusTableCellComponentProps,
): JSX.Element => {
    return (
        <Tooltip title={<TooltipContent {...props} />}>
            <span style={{ display: 'inline-flex', alignItems: 'center' }}>
                <StatusIcon status={props.status} />
            </span>
        </Tooltip>
    );
};
